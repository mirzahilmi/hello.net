using FluentValidation;
using Hello.NET.Domain.DTOs;

namespace Hello.NET.Validators;

public sealed class ArticleCreateRequestValidator
    : AbstractValidator<ArticleCreateRequest>
{
    public ArticleCreateRequestValidator()
    {
        RuleFor(article => article.Title)
            .NotEmpty()
            .WithMessage("{PropertyName} is required");
        RuleFor(article => article.Slug)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .Matches("^[a-z0-9]+(?:.-[a-z0-9]+)*$")
            .WithMessage("{PropertyName} must be valid slug");
        RuleFor(article => article.Content)
            .NotEmpty()
            .WithMessage("{PropertyName} is required");
        RuleFor(article => article.PublishedAt)
            .NotEmpty()
            .WithMessage("{PropertyName} is required");
        RuleForEach(article => article.Categories)
            .ChildRules(category =>
            {
                category.RuleFor(category => category.Name).NotEmpty();
            });
    }
}
