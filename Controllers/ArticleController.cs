namespace hello.net.Controllers;
using hello.net.Data;
using hello.net.Models;
using hello.net.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("/api/articles")]
public class ArticleController(
    ApplicationDbContext context,
    ILogger<ArticleController> logger
) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<ArticleController> _logger = logger;

    [HttpGet]
    [ProducesResponseType<IEnumerable<Article>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Article>>> GetArticles() =>
        await _context.Articles.ToListAsync();

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<Article>(StatusCodes.Status200OK)]
    public async Task<ActionResult<Article>> GetArticle(ulong id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article == null)
            return NotFound();
        return article;
    }

    [HttpPost]
    [ProducesResponseType<Article>(StatusCodes.Status201Created)]
    public async Task<ActionResult<Article>> PostArticle(
        ArticleCreateRequestModel article
    )
    {
        var _article = new Article
        {
            Title = article.Title,
            Slug = article.Slug,
            Content = article.Content,
            PublishedAt = article.PublishedAt,
        };
        _context.Articles.Add(_article);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Article {Article} created", _article.Title);

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
        ulong id,
        ArticleUpdateRequestModel article
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

        _logger.LogInformation("Article {Article} updated", _article.Title);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteArticle(ulong id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article == null)
            return NotFound();

        _context.Articles.Remove(article);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Article {Article} deleted", article.Title);

        return NoContent();
    }
}
