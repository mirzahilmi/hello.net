namespace Hello.NET.Controllers;

using Asp.Versioning;
using FluentValidation;
using Hello.NET.Domain.DTOs;
using Hello.NET.Domain.Services;
using Hello.NET.Filters;
using Hello.NET.Mapping.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{version:apiVersion}/articles")]
public class ArticleController(
    IArticleService service,
    IArticleMapper mapper,
    IValidator<ArticleDto> validator,
    ILogger<ArticleController> logger
) : ControllerBase
{
    private readonly IArticleService _service = service;
    private readonly IArticleMapper _mapper = mapper;
    private readonly IValidator<ArticleDto> _validator = validator;
    private readonly ILogger<ArticleController> _logger = logger;

    [HttpGet]
    [MapToApiVersion(1.0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<
        ActionResult<IEnumerable<ArticleResourceResponse>>
    > GetArticles([FromQuery] PagingDto paging)
    {
        _logger.LogDebug(
            "Getting article with page index of {pageIndex} and page size of {pageSize}",
            paging.PageIndex,
            paging.PageSize
        );
        var articles = await _service.RetrieveArticlesAsync(paging);
        _logger.LogInformation(
            "Received {count} articles from the query",
            articles.Count
        );

        return Ok(articles);
    }

    [HttpGet("stream")]
    [MapToApiVersion(1.0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async IAsyncEnumerable<ArticleResourceResponse> GetArticlesStream(
        [FromQuery] PagingDto paging
    )
    {
        _logger.LogDebug(
            "Getting article with page index of {pageIndex} and page size of {pageSize}",
            paging.PageIndex,
            paging.PageSize
        );
        var articles = await _service.RetrieveArticlesAsync(paging);
        _logger.LogInformation(
            "Received {count} articles from the query",
            articles.Count
        );

        foreach (var article in articles)
            yield return article;
    }

    [HttpGet("{id}")]
    [MapToApiVersion(1.0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ArticleResourceResponse>> GetArticle(
        [FromRoute] long id
    )
    {
        _logger.LogDebug("Getting article with id of {articleID}", id);
        var article = await _service.RetrieveArticleAsync(id);
        if (article == null)
        {
            _logger.LogError("Cannot find article with id of {articleID}", id);
            return NotFound();
        }
        _logger.LogInformation(
            "Received article with id of {articleID} and title of {title}",
            article.ID,
            article.Title
        );

        return article;
    }

    [HttpPost]
    [MapToApiVersion(1.0)]
    [ServiceFilter<InputValidationFilter<ArticleDto>>]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<ArticleResourceResponse>> PostArticle(
        [FromBody] ArticleDto article
    )
    {
        _logger.LogDebug(
            "Creating article with title of {title}",
            article.Title
        );
        var id = await _service.CreateArticleAsync(article);
        _logger.LogInformation(
            "Created article with id of {articleID} and title of {title}",
            id,
            article.Title
        );
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
        _logger.LogDebug(
            "Updating article with id of {articleID} and title of {title}",
            id,
            article.Title
        );
        await _service.UpdateArticleAsync(id, article);
        _logger.LogInformation(
            "Updated article with id of {articleID} and title of {title}",
            id,
            article.Title
        );
        return NoContent();
    }

    [HttpDelete("{id}")]
    [MapToApiVersion(1.0)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteArticle([FromRoute] long id)
    {
        _logger.LogDebug("Deleting article with id of {articleID}", id);
        await _service.DeleteArticleAsync(id);
        _logger.LogInformation("Deleted article with id of {articleID}", id);
        return NoContent();
    }
}
