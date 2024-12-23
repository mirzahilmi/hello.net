using Hello.NET.Domain.DTOs;
using Hello.NET.Models;

namespace Hello.NET.Mapping.Interfaces;

public interface IArticleMapper {
    public Article? Map(ArticleDto article);
    public ArticleDto? Map(Article article);
}
