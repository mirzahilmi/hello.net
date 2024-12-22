using Hello.NET.Domain.DTOs;
using Hello.NET.Models;

namespace Hello.NET.Mapping.Interfaces;

public interface IArticleDtoMapper {
    public Article? Map(ArticleDto article);
}
