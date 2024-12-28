using Hello.NET.Domain.DTOs;
using Hello.NET.Infrastructure.SQL.Database.Entities;

namespace Hello.NET.Mapping.Interfaces;

public interface IArticleMapper {
    public ArticleEntity? Map(ArticleDto article);
    public ArticleDto? Map(ArticleEntity article);
    public List<ArticleDto> Map(List<ArticleEntity> articles);
    public List<ArticleEntity> Map(List<ArticleDto> articles);
}
