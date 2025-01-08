using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Hello.NET.Domain.DTOs;
using Hello.NET.Domain.Repositories;
using Hello.NET.Exceptions;
using Hello.NET.Infrastructure.SQL.Database;
using Hello.NET.Infrastructure.SQL.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Hello.NET.Infrastructure.SQL.Repositories;

public sealed class ArticleRepository(
    ApplicationDbContext context,
    ElasticsearchClient elasticClient
) : IArticleRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<ArticleEntity?> GetArticleAsync(long id) =>
        await _context
            .Articles.AsNoTracking()
            .Include(article => article.Categories)
            .FirstOrDefaultAsync(article => article.ID == id);

    public async Task<List<ArticleEntity>> GetArticlesAsync(PagingDto paging) =>
        await _context
            .Articles.AsNoTracking()
            .Include(article => article.Categories)
            .Skip((paging.PageIndex - 1) * paging.PageSize)
            .Take(paging.PageSize)
            .ToListAsync();

    public async Task<List<ArticleEntity>> GetArticlesAsync(
        ArticleSearchQuery query
    )
    {
        var request = new SearchRequest("articles")
        {
            Query = new MatchQuery(new Field("content"))
            {
                Query = query.Query,
            },
        };
        var response = await elasticClient.SearchAsync<ArticleEntity>(request);
        if (!response.IsValidResponse)
            return [];
        return response.Documents.ToList();
    }

    public async Task<ArticleEntity> CreateArticleAsync(ArticleEntity article)
    {
        await _context.Articles.AddAsync(article);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is NpgsqlException _ex
                && _ex.SqlState == "23505"
            )
        {
            throw new DataConflictException("Slug already exists");
        }
        return article;
    }

    public async Task<bool> CheckArticleAsync(long id) =>
        await _context.Articles.AnyAsync(article => article.ID == id);

    public async Task<int> UpdateArticleAsync(long id, ArticleEntity article) =>
        await _context
            .Articles.Where(_article => _article.ID == id)
            .ExecuteUpdateAsync(setters =>
                setters
                    .SetProperty(_article => _article.Title, article.Title)
                    .SetProperty(_article => _article.Slug, article.Slug)
                    .SetProperty(_article => _article.Content, article.Content)
                    .SetProperty(
                        _article => _article.PublishedAt,
                        article.PublishedAt
                    )
            );

    public async Task<int> DeleteArticleAsync(long id) =>
        await _context
            .Articles.Where(article => article.ID == id)
            .ExecuteDeleteAsync();
}
