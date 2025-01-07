using System.Text.Json;
using Hello.NET.Domain.DTOs;
using Hello.NET.Domain.Repositories;
using Hello.NET.Domain.Services;
using Hello.NET.Infrastructure.SQL.Database;
using Hello.NET.Infrastructure.SQL.Database.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Hello.NET.Usecase.Services;

public sealed class ArticleService(
    IArticleRepository repository,
    Transaction transaction,
    IDistributedCache cache
) : IArticleService
{
    public IArticleRepository _repository = repository;

    public async Task<List<ArticleResourceResponse>> RetrieveArticlesAsync(
        PagingDto paging
    )
    {
        var cached = await cache.GetStringAsync("articles");
        if (!string.IsNullOrEmpty(cached))
        {
            var cachedArticles = JsonSerializer.Deserialize<
                List<ArticleResourceResponse>
            >(cached);
            return cachedArticles ?? [];
        }

        var result = await _repository.GetArticlesAsync(paging);

        var articles = result.ConvertAll(article =>
            article.ToArticleResponse(article.ID)
        );

        var serialized = JsonSerializer.Serialize(articles);
        await cache.SetStringAsync("articles", serialized);

        return articles;
    }

    public async Task<ArticleResourceResponse?> RetrieveArticleAsync(long id)
    {
        var cached = await cache.GetStringAsync($"articles:${id}");
        if (!string.IsNullOrEmpty(cached))
        {
            var cachedArticle =
                JsonSerializer.Deserialize<ArticleResourceResponse>(cached);
            return cachedArticle;
        }

        var result = await _repository.GetArticleAsync(id);
        if (result == null)
            return null;

        var article = result.ToArticleResponse(result.ID);

        var serialized = JsonSerializer.Serialize(article);
        await cache.SetStringAsync($"articles:${id}", serialized);

        return article;
    }

    public async Task<ArticleResourceResponse> CreateArticleAsync(
        ArticleCreateRequest payload
    )
    {
        var result = await transaction.ExecuteAsync(async () =>
        {
            var article = payload.ToArticleEntity();
            article = await _repository.CreateArticleAsync(article);
            return article;
        });

        var article = result.ToArticleResponse(result.ID);

        var cached = await cache.GetStringAsync("articles");
        List<ArticleResourceResponse> articles = [];
        if (!string.IsNullOrEmpty(cached))
            articles =
                JsonSerializer.Deserialize<List<ArticleResourceResponse>>(
                    cached
                ) ?? [];

        articles.Add(article);
        var serialized = JsonSerializer.Serialize(articles);
        await cache.SetStringAsync("articles", serialized);

        return article;
    }

    public async Task<ArticleResourceResponse> UpdateArticleAsync(
        long id,
        ArticleDto article
    )
    {
        await _repository.UpdateArticleAsync(id, article.ToArticleEntity());

        var cached = await cache.GetStringAsync("articles");

        List<ArticleResourceResponse> articles = [];
        if (!string.IsNullOrEmpty(cached))
            articles =
                JsonSerializer.Deserialize<List<ArticleResourceResponse>>(
                    cached
                ) ?? [];

        var articleResponse = article.ToArticleResponse(id);
        var i = articles.FindIndex(article => article.ID == id);
        if (i != -1)
            // BUG: replaces categories with from request payload
            articles[i] = articleResponse;

        var serializedArticles = JsonSerializer.Serialize(articles);
        await cache.SetStringAsync("articles", serializedArticles);
        var serializedArticle = JsonSerializer.Serialize(articleResponse);
        await cache.SetStringAsync($"article:{id}", serializedArticle);

        return article.ToArticleResponse(id);
    }

    public async Task DeleteArticleAsync(long id)
    {
        await _repository.DeleteArticleAsync(id);

        var cached = await cache.GetStringAsync("articles");

        List<ArticleResourceResponse> articles = [];
        if (!string.IsNullOrEmpty(cached))
            articles =
                JsonSerializer.Deserialize<List<ArticleResourceResponse>>(
                    cached
                ) ?? [];

        articles.RemoveAll(article => article.ID == id);
        var serializedArticles = JsonSerializer.Serialize(articles);
        await cache.SetStringAsync("articles", serializedArticles);
    }
}
