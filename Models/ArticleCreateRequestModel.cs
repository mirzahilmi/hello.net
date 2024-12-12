namespace hello.net.Models;

public class ArticleCreateRequestModel
{
    public required string Title { get; set; }
    public required string Slug { get; set; }
    public required string Content { get; set; }
    public required DateTime PublishedAt { get; set; }
}
