using Hello.NET.Domain.DTOs;
using Hello.NET.Infrastructure.SQL.Database.Entities;

namespace Hello.NET.Domain.Repositories;

public interface ICategoryRepository
{
    Task<List<CategoryEntity>> GetCategoriesAsync(PagingDto paging);
    Task<CategoryEntity?> GetCategoryAsync(long id);
    Task<bool> CheckCategoryAsync(long id);
    Task<long> CreateCategoryAsync(CategoryEntity category);
    Task<int> UpdateCategoryAsync(long id, CategoryEntity category);
    Task<int> DeleteCategoryAsync(long id);
}
