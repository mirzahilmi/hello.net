using Hello.NET.Domain.DTOs;
using Hello.NET.Infrastructure.SQL.Database.Entities;

namespace Hello.NET.Domain.Services;

public interface ICategoryService
{
    Task<List<CategoryResourceResponse>> RetrieveCategoriesAsync(
        PagingDto paging
    );
    Task<CategoryResourceResponse?> RetrieveCategoryAsync(long id);
    Task<CategoryResourceResponse> CreateCategoryAsync(
        CategoryCreateRequest category
    );
    Task<List<CategoryResourceResponse>> CreateCategoriesAsync(
        List<CategoryEntity> categories
    );
    Task<CategoryResourceResponse> UpdateCategoryAsync(
        long id,
        CategoryUpdateRequest category
    );
    Task DeleteCategoryAsync(long id);
}
