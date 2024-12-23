namespace Hello.NET.Controllers;

using FluentValidation;
using Hello.NET.Domain.DTOs;
using Hello.NET.Domain.Services;
using Hello.NET.Mapping.Interfaces;
using Hello.NET.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/articles")]
public class ArticleController(
    IArticleService service,
    IArticleMapper mapper,
    IValidator<ArticleDto> validator
) : ControllerBase
{
    private readonly IArticleService _service = service;
    private readonly IArticleMapper _mapper = mapper;
    private readonly IValidator<ArticleDto> _validator = validator;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Article>>> GetArticles() =>
        await _service.GetArticlesAsync();

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Article>> GetArticle([FromRoute] long id)
    {
        var article = await _service.GetArticleAsync(id);
        if (article == null)
            return NotFound();

        return article;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Article>> PostArticle(
        [FromBody] ArticleDto article
    )
    {
        var result = await _validator.ValidateAsync(article);
        if (!result.IsValid)
            return ValidationProblem(
                new ValidationProblemDetails(result.ToDictionary())
            );

        var _article = _mapper.Map(article);
        if (_article == null)
            return BadRequest();

        await _service.CreateArticleAsync(_article);

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
        [FromRoute] long id,
        [FromBody] ArticleDto article
    )
    {
        var result = await _validator.ValidateAsync(article);
        if (!result.IsValid)
            return ValidationProblem(
                new ValidationProblemDetails(result.ToDictionary())
            );

        var _article = _mapper.Map(article);
        if (_article == null)
            return BadRequest();

        var exist = await _service.CheckArticleAsync(id);
        if (!exist)
            return NotFound();

        await _service.UpdateArticleAsync(id, _article);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteArticle([FromRoute] long id)
    {
        var exist = await _service.CheckArticleAsync(id);
        if (!exist)
            return NotFound();

        await _service.DeleteArticleAsync(id);

        return NoContent();
    }
}
