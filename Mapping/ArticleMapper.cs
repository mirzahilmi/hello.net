using Hello.NET.Domain.DTOs;
using Hello.NET.Infrastructure.SQL.Database.Entities;
using Hello.NET.Mapping.Interfaces;

namespace Hello.NET.Mapping;

public class ArticleMapper : IArticleMapper
{
    public ArticleEntity? Map(ArticleDto article)
    {
        return article is not null
            ? new ArticleEntity
            {
                Title = article.Title,
                Slug = article.Slug,
                Content = article.Content,
                PublishedAt = article.PublishedAt,
            }
            : null;
    }

    public ArticleDto? Map(ArticleEntity article)
    {
        return article is not null
            ? new ArticleDto
            {
                Title = article.Title,
                Slug = article.Slug,
                Content = article.Content,
                PublishedAt = article.PublishedAt,
            }
            : null;
    }
}
