namespace Hello.NET.Domain.DTOs;

public sealed record class CategoryCreateRequest
{
    public required string Name { get; set; }
}
