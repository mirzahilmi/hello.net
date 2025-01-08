namespace Hello.NET.Domain.DTOs;

public sealed record class ArticleSearchQuery
{
    public string Query { get; set; } = string.Empty;
    // TODO: allow for search query with filtering
    // public string Categories { get; set; } = string.Empty;
}
