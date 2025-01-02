using Hello.NET.Domain.DTOs;
using Hello.NET.Domain.Repositories;
using Hello.NET.Domain.Services;
using Hello.NET.Exceptions;
using Hello.NET.Infrastructure.SQL.Database.Entities;

namespace Hello.NET.Usecase.Services;

public sealed class CategoryService(
    ICategoryRepository repository,
    ILogger<CategoryService> logger
) : ICategoryService
{
    private readonly ICategoryRepository _repository = repository;
    private readonly ILogger<CategoryService> _logger = logger;

    public async Task<List<CategoryResourceResponse>> RetrieveCategoriesAsync(
        PagingDto paging
    )
    {
        var categories = await _repository.GetCategoriesAsync(paging);
        return categories.ConvertAll(category =>
            category.ToCategoryResponse(category.ID)
        );
    }

    public async Task<CategoryResourceResponse?> RetrieveCategoryAsync(long id)
    {
        var category = await _repository.GetCategoryAsync(id);
        return category?.ToCategoryResponse(id);
    }

    public async Task<CategoryResourceResponse> CreateCategoryAsync(
        CategoryCreateRequest category
    )
    {
        var entity = category.ToCategoryEntity();
        var id = await _repository.CreateCategoryAsync(entity);
        return entity.ToCategoryResponse(id);
    }

    public async Task<CategoryResourceResponse> UpdateCategoryAsync(
        long id,
        CategoryUpdateRequest category
    )
    {
        var affected = await _repository.UpdateCategoryAsync(
            id,
            category.ToCategoryEntity()
        );
        if (affected == 0)
        {
            _logger.LogError(
                "Attempt to update category of id {ID}, no rows affected",
                id
            );
            throw new DataConflictException("No category have been updated");
        }

        return category.ToCategoryResponse(id);
    }

    public async Task DeleteCategoryAsync(long id) =>
        await _repository.DeleteCategoryAsync(id);
}
