using Hello.NET.Domain.DTOs;

namespace Hello.NET.Infrastructure.SQL.Database.Entities;

public static class EntityExtensions
{
    public static CategoryResourceResponse ToCategoryResponse(
        this CategoryEntity category, long id
    ) => new() { ID = id, Name = category.Name };

    public static CategoryResourceResponse ToCategoryResponse(
        this CategoryUpdateRequest category, long id
    ) => new() { ID = id, Name = category.Name };

    public static CategoryEntity ToCategoryEntity(
        this CategoryCreateRequest category
    ) => new() { Name = category.Name };

    public static CategoryEntity ToCategoryEntity(
        this CategoryUpdateRequest category
    ) => new() { Name = category.Name };
}
