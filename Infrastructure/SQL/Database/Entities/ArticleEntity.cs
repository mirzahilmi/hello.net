namespace Hello.NET.Infrastructure.SQL.Database.Entities;

using System.ComponentModel.DataAnnotations;

public record class ArticleEntity
{
    [Key]
    public long ID { get; set; }
    public required string Title { get; set; }
    public required string Slug { get; set; }
    public required string Content { get; set; }
    public required DateTime PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
