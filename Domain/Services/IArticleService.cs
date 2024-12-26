using Hello.NET.Infrastructure.SQL.Database.Entities;

namespace Hello.NET.Domain.Services;

public interface IArticleService
{
    Task<List<ArticleEntity>> GetArticlesAsync();
    Task<ArticleEntity?> GetArticleAsync(long id);
    Task<bool> CheckArticleAsync(long id);
    Task<long> CreateArticleAsync(ArticleEntity article);
    Task UpdateArticleAsync(long id, ArticleEntity article);
    Task DeleteArticleAsync(long id);
}
