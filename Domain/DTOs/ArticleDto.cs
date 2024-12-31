using System.Text.Json.Serialization;

namespace Hello.NET.Domain.DTOs;

public record class ArticleDto
{
    [JsonIgnore]
    public long? ID { get; set; }
    public required string Title { get; set; }
    public required string Slug { get; set; }
    public required string Content { get; set; }
    public required DateTime PublishedAt { get; set; }
}
