namespace Hello.NET.Domain.DTOs;

public record class ArticleCreateRequest
{
    public required string Title { get; set; }
    public required string Slug { get; set; }
    public required string Content { get; set; }
    public required DateTime PublishedAt { get; set; }
    public required List<CategoryCreateRequest> Categories { get; set; } = [];
}
