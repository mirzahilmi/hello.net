using Hello.NET.Domain.DTOs;

namespace Hello.NET.Domain.Services;

public interface IArticleService
{
    Task<List<ArticleResourceResponse>> RetrieveArticlesAsync(PagingDto paging);
    Task<ArticleResourceResponse?> RetrieveArticleAsync(long id);
    Task<ArticleResourceResponse> CreateArticleAsync(ArticleCreateRequest article);
    Task<ArticleResourceResponse> UpdateArticleAsync(
        long id,
        ArticleDto article
    );
    Task DeleteArticleAsync(long id);
}
