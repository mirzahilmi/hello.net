using Hello.NET.Domain.DTOs;

namespace Hello.NET.Domain.Services;

public interface IArticleService
{
    Task<List<ArticleDto>> RetrieveUsersAsync(PagingDto paging);
    Task<ArticleDto?> RetrieveUserAsync(long id);
    Task<long> CreateArticleAsync(ArticleDto article);
    Task<int> UpdateArticleAsync(ArticleDto article);
    Task<int> DeleteArticleAsync(long id);
}