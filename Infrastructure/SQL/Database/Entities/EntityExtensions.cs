using Hello.NET.Domain.DTOs;

namespace Hello.NET.Infrastructure.SQL.Database.Entities;

public static class EntityExtensions
{
    public static CategoryResourceResponse ToCategoryResponse(
        this CategoryEntity category,
        long id
    ) => new() { ID = id, Name = category.Name };

    public static CategoryResourceResponse ToCategoryResponse(
        this CategoryUpdateRequest category,
        long id
    ) => new() { ID = id, Name = category.Name };

    public static CategoryEntity ToCategoryEntity(
        this CategoryCreateRequest category
    ) => new() { Name = category.Name };

    public static CategoryEntity ToCategoryEntity(
        this CategoryUpdateRequest category
    ) => new() { Name = category.Name };

    public static ArticleResourceResponse ToArticleResponse(
        this ArticleEntity article,
        long id
    ) =>
        new()
        {
            ID = id,
            Title = article.Title,
            Slug = article.Slug,
            Content = article.Content,
            PublishedAt = article.PublishedAt,
            Categories = article.Categories.ConvertAll(category =>
                category.ToCategoryResponse(category.ID)
            ),
        };

    public static ArticleResourceResponse ToArticleResponse(
        this ArticleDto article,
        long id
    ) =>
        new()
        {
            ID = id,
            Title = article.Title,
            Slug = article.Slug,
            Content = article.Content,
            PublishedAt = article.PublishedAt,
        };

    public static ArticleResourceResponse ToArticleResponse(
        this ArticleCreateRequest article,
        long id
    ) =>
        new()
        {
            ID = id,
            Title = article.Title,
            Slug = article.Slug,
            Content = article.Content,
            PublishedAt = article.PublishedAt,
        };

    public static ArticleEntity ToArticleEntity(this ArticleDto article) =>
        new()
        {
            Title = article.Title,
            Slug = article.Slug,
            Content = article.Content,
            PublishedAt = article.PublishedAt,
        };

    public static ArticleEntity ToArticleEntity(
        this ArticleCreateRequest article
    ) =>
        new()
        {
            Title = article.Title,
            Slug = article.Slug,
            Content = article.Content,
            PublishedAt = article.PublishedAt,
            Categories = article.Categories.ConvertAll(category =>
                category.ToCategoryEntity()
            ),
        };
}
