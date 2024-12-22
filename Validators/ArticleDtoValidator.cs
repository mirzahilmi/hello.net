using FluentValidation;
using Hello.NET.Domain.DTOs;

namespace Hello.NET.Validators;

public class ArticleDtoValidator : AbstractValidator<ArticleDto>
{
    public ArticleDtoValidator()
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
    }
}

