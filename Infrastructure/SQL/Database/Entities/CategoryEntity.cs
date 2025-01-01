namespace Hello.NET.Infrastructure.SQL.Database.Entities;

using System.ComponentModel.DataAnnotations;

public record class CategoryEntity
{
    [Key]
    public long ID { get; set; }
    public required string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<ArticleEntity> Articles { get; set; } = [];
}
