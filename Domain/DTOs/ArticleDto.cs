namespace Hello.NET.Domain.DTOs;

public class ArticleDto
{
    public long? ID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime PublishedAt { get; set; }
}
