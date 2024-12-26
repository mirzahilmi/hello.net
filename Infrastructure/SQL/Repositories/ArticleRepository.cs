using Hello.NET.Domain.Repositories;
using Hello.NET.Infrastructure.SQL.Database;
using Hello.NET.Infrastructure.SQL.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hello.NET.Infrastructure.SQL.Repositories;

public class ArticleRepository(ApplicationDbContext context)
    : IArticleRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<ArticleEntity?> GetArticleAsync(long id) =>
        await _context
            .Articles.AsNoTracking()
            .FirstAsync(article => article.ID == id);

    public async Task<List<ArticleEntity>> GetArticlesAsync() =>
        await _context.Articles.AsNoTracking().ToListAsync();

    public async Task<long> CreateArticleAsync(ArticleEntity article)
    {
        _context.Articles.Add(article);
        await _context.SaveChangesAsync();
        return article.ID;
    }

    public async Task<bool> CheckArticleAsync(long id) =>
        await _context
            .Articles.AsNoTracking()
            .AnyAsync(article => article.ID == id);

    public async Task<int> UpdateArticleAsync(long id, ArticleEntity article) =>
        await _context
            // also not sure
            .Articles.AsNoTracking()
            .Where(_article => _article.ID == id)
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
            // not sure disabling tracking here add significant difference
            .Articles.AsNoTracking()
            .Where(article => article.ID == id)
            .ExecuteDeleteAsync();
}
