using Hello.NET.Data;
using Hello.NET.Models;
using Microsoft.EntityFrameworkCore;

namespace Hello.NET.Domain.Services;

public class ArticleService(ApplicationDbContext context) : IArticleService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> CheckArticleAsync(long id) =>
        await _context.Articles.AnyAsync(article => article.ID == id);

    public async Task<long> CreateArticleAsync(Article article)
    {
        _context.Articles.Add(article);
        await _context.SaveChangesAsync();
        return article.ID;
    }

    public async Task DeleteArticleAsync(long id) =>
        await _context
            .Articles.Where(article => article.ID == id)
            .ExecuteDeleteAsync();

    public async Task<Article?> GetArticleAsync(long id) =>
        await _context.Articles.FindAsync(id);

    public async Task<List<Article>> GetArticlesAsync() =>
        await _context.Articles.ToListAsync();

    public async Task UpdateArticleAsync(long id, Article article) =>
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
}
