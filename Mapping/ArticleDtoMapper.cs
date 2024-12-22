using Hello.NET.Domain.DTOs;
using Hello.NET.Models;
using Hello.NET.Mapping.Interfaces;

namespace Hello.NET.Mapping;

public class ArticleDtoMapper : IArticleDtoMapper
{
    public Article? Map(ArticleDto article)
    {
        return article is not null
            ? new Article
            {
                Title = article.Title,
                Slug = article.Slug,
                Content = article.Content,
                PublishedAt = article.PublishedAt,
            }
            : null;
    }
}
