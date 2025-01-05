namespace Hello.NET.Domain.DTOs;

public sealed class PagingDto
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 15;
}
