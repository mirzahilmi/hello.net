namespace Hello.NET.Controllers;

using Asp.Versioning;
using FluentValidation;
using Hello.NET.Domain.DTOs;
using Hello.NET.Domain.Repositories;
using Hello.NET.Filters;
using Hello.NET.Mapping.Interfaces;
using Hello.NET.Infrastructure.SQL.Database.Entities;
using Microsoft.AspNetCore.Mvc;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{version:apiVersion}/articles")]
public class ArticleController(
    IArticleRepository service,
    IArticleMapper mapper,
    IValidator<ArticleDto> validator
) : ControllerBase
{
    private readonly IArticleRepository _service = service;
    private readonly IArticleMapper _mapper = mapper;
    private readonly IValidator<ArticleDto> _validator = validator;

    [HttpGet]
    [MapToApiVersion(1.0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ArticleEntity>>> GetArticles() =>
        await _service.GetArticlesAsync();

    [HttpGet("{id}")]
    [MapToApiVersion(1.0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ArticleEntity>> GetArticle([FromRoute] long id)
    {
        var article = await _service.GetArticleAsync(id);
        if (article == null)
            return NotFound();

        return article;
    }

    [HttpPost]
    [MapToApiVersion(1.0)]
    [ServiceFilter<InputValidationFilter<ArticleDto>>]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<ArticleEntity>> PostArticle(
        [FromBody] ArticleDto article
    )
    {
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
    [MapToApiVersion(1.0)]
    [ServiceFilter<InputValidationFilter<ArticleDto>>]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutArticle(
        [FromRoute] long id,
        [FromBody] ArticleDto article
    )
    {
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
    [MapToApiVersion(1.0)]
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
