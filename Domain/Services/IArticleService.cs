using Hello.NET.Models;

namespace Hello.NET.Domain.Services;

public interface IArticleService
{
    Task<List<Article>> GetArticlesAsync();
    Task<Article?> GetArticleAsync(long id);
    Task<bool> CheckArticleAsync(long id);
    Task<long> CreateArticleAsync(Article article);
    Task UpdateArticleAsync(long id, Article article);
    Task DeleteArticleAsync(long id);
}
