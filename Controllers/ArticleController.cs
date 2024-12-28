namespace Hello.NET.Controllers;

using Asp.Versioning;
using FluentValidation;
using Hello.NET.Domain.DTOs;
using Hello.NET.Domain.Services;
using Hello.NET.Filters;
using Hello.NET.Infrastructure.SQL.Database.Entities;
using Hello.NET.Mapping.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{version:apiVersion}/articles")]
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
    [MapToApiVersion(1.0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<ArticleDto>> GetArticles(
        [FromQuery] PagingDto paging
    ) => await _service.RetrieveUsersAsync(paging);

    [HttpGet("stream")]
    [MapToApiVersion(1.0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async IAsyncEnumerable<ArticleDto> GetArticlesStream(
        [FromQuery] PagingDto paging
    )
    {
        var articles = await _service.RetrieveUsersAsync(paging);
        foreach (var article in articles)
            yield return article;
    }

    [HttpGet("{id}")]
    [MapToApiVersion(1.0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ArticleDto>> GetArticle([FromRoute] long id)
    {
        var article = await _service.RetrieveUserAsync(id);
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
        var id = await _service.CreateArticleAsync(article);

        return CreatedAtAction(nameof(GetArticle), new { id }, article);
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
        article.ID = id;
        await _service.UpdateArticleAsync(article);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [MapToApiVersion(1.0)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteArticle([FromRoute] long id)
    {
        await _service.DeleteArticleAsync(id);
        return NoContent();
    }
}
