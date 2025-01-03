using Hello.NET.Domain.DTOs;
using Hello.NET.Domain.Repositories;
using Hello.NET.Domain.Services;
using Hello.NET.Infrastructure.SQL.Database.Entities;

namespace Hello.NET.Usecase.Services;

public class ArticleService(IArticleRepository repository) : IArticleService
{
    public IArticleRepository _repository = repository;

    public async Task<List<ArticleResourceResponse>> RetrieveArticlesAsync(
        PagingDto paging
    )
    {
        var articles = await _repository.GetArticlesAsync(paging);
        return articles.ConvertAll(article =>
            article.ToArticleResponse(article.ID)
        );
    }

    public async Task<ArticleResourceResponse?> RetrieveArticleAsync(long id)
    {
        var article = await _repository.GetArticleAsync(id);
        if (article == null)
            return null;

        return article.ToArticleResponse(article.ID);
    }

    public async Task<ArticleResourceResponse> CreateArticleAsync(
        ArticleDto article
    )
    {
        var id = await _repository.CreateArticleAsync(
            article.ToArticleEntity()
        );
        return article.ToArticleResponse(id);
    }

    public async Task<ArticleResourceResponse> UpdateArticleAsync(
        long id,
        ArticleDto article
    )
    {
        await _repository.UpdateArticleAsync(id, article.ToArticleEntity());
        return article.ToArticleResponse(id);
    }

    public async Task DeleteArticleAsync(long id) =>
        await _repository.DeleteArticleAsync(id);
}
