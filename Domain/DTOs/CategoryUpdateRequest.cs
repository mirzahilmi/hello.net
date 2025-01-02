namespace Hello.NET.Domain.DTOs;

public sealed record class CategoryUpdateRequest
{
    public required string Name { get; set; }
}
