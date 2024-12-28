using Hello.NET.Domain.DTOs;
using Hello.NET.Domain.Repositories;
using Hello.NET.Domain.Services;
using Hello.NET.Mapping.Interfaces;

namespace Hello.NET.Usecase.Services;

public class ArticleService(
    IArticleRepository repository,
    IArticleMapper mapper
) : IArticleService
{
    public IArticleRepository _repository = repository;
    public IArticleMapper _mapper = mapper;

    public async Task<List<ArticleDto>> RetrieveUsersAsync(PagingDto paging) =>
        _mapper.Map(await _repository.GetArticlesAsync(paging));

    public async Task<ArticleDto?> RetrieveUserAsync(long id)
    {
        var article = await _repository.GetArticleAsync(id);
        if (article == null)
            return null;

        return _mapper.Map(article);
    }

    public async Task<long> CreateArticleAsync(ArticleDto article)
    {
        var _article = _mapper.Map(article);
        if (_article == null)
            return 0L;

        return await _repository.CreateArticleAsync(_article);
    }

    public async Task<int> UpdateArticleAsync(ArticleDto article)
    {
        var _article = _mapper.Map(article);
        if (_article == null)
            return 0;

        return await _repository.UpdateArticleAsync(_article.ID, _article);
    }

    public async Task<int> DeleteArticleAsync(long id) =>
        await _repository.DeleteArticleAsync(id);
}
