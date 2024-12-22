namespace Hello.NET.Controllers;

using FluentValidation;
using Hello.NET.Data;
using Hello.NET.Domain.DTOs;
using Hello.NET.Mapping.Interfaces;
using Hello.NET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("/api/articles")]
public class ArticleController(
    ApplicationDbContext context,
    IArticleDtoMapper mapper,
    IValidator<ArticleDto> validator
) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;
    private readonly IArticleDtoMapper _mapper = mapper;
    private readonly IValidator<ArticleDto> _validator = validator;

    [HttpGet]
    [ProducesResponseType<IEnumerable<Article>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Article>>> GetArticles() =>
        await _context.Articles.ToListAsync();

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<Article>(StatusCodes.Status200OK)]
    public async Task<ActionResult<Article>> GetArticle([FromRoute] ulong id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article == null)
            return NotFound();
        return article;
    }

    [HttpPost]
    [ProducesResponseType<Article>(StatusCodes.Status201Created)]
    public async Task<ActionResult<Article>> PostArticle(
        [FromBody] ArticleDto article
    )
    {
        var result = _validator.Validate(article);
        if (!result.IsValid)
            return UnprocessableEntity();

        var _article = _mapper.Map(article);
        if (_article == null)
            return BadRequest();

        _context.Articles.Add(_article);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetArticle),
            new { id = _article.ID },
            _article
        );
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutArticle(
        [FromRoute] ulong id,
        [FromBody] ArticleDto article
    )
    {
        var _article = await _context.Articles.FindAsync(id);
        if (_article == null)
            return NotFound();

        _article.Title = article.Title;
        _article.Slug = article.Slug;
        _article.Content = article.Content;
        _article.PublishedAt = article.PublishedAt;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteArticle([FromRoute] ulong id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article == null)
            return NotFound();

        _context.Articles.Remove(article);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
