namespace Hello.NET.Domain.DTOs;

public sealed record class CategoryResourceResponse
{
    public long ID { get; set; }
    public required string Name { get; set; }
}
