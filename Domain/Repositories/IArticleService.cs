using Hello.NET.Domain.DTOs;
using Hello.NET.Infrastructure.SQL.Database.Entities;

namespace Hello.NET.Domain.Repositories;

public interface IArticleRepository
{
    Task<List<ArticleEntity>> GetArticlesAsync(PagingDto paging);
    Task<ArticleEntity?> GetArticleAsync(long id);
    Task<bool> CheckArticleAsync(long id);
    Task<long> CreateArticleAsync(ArticleEntity article);
    Task<int> UpdateArticleAsync(long id, ArticleEntity article);
    Task<int> DeleteArticleAsync(long id);
}
