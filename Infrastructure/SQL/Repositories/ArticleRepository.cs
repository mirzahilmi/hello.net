using Hello.NET.Domain.DTOs;
using Hello.NET.Domain.Repositories;
using Hello.NET.Exceptions;
using Hello.NET.Infrastructure.SQL.Database;
using Hello.NET.Infrastructure.SQL.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Hello.NET.Infrastructure.SQL.Repositories;

public class ArticleRepository(
    ApplicationDbContext context,
    ILogger<ArticleRepository> logger
) : IArticleRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<ArticleRepository> _logger = logger;

    public async Task<ArticleEntity?> GetArticleAsync(long id) =>
        await _context
            .Articles.AsNoTracking()
            .FirstOrDefaultAsync(article => article.ID == id);

    public async Task<List<ArticleEntity>> GetArticlesAsync(PagingDto paging) =>
        await _context
            .Articles.AsNoTracking()
            .Skip((paging.PageIndex - 1) * paging.PageSize)
            .Take(paging.PageSize)
            .ToListAsync();

    public async Task<long> CreateArticleAsync(ArticleEntity article)
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
        return article.ID;
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
