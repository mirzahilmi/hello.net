namespace Hello.NET.Domain.DTOs;

public sealed record class CreateCategoryRequest
{
    public required string Name { get; set; }
}
