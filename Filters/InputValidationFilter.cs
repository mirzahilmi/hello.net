using FluentValidation;
using Hello.NET.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hello.NET.Filters;

public class InputValidationFilter(IValidator<ArticleDto> validator)
    : IAsyncActionFilter
{
    private readonly IValidator<ArticleDto> _validator = validator;

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        context.ActionArguments.TryGetValue("article", out var arg);
        if (arg is not ArticleDto article)
        {
            context.Result = new BadRequestResult();
            return;
        }

        var result = await _validator.ValidateAsync(article);
        if (!result.IsValid)
        {
            result.Errors.ForEach(err =>
            {
                context.ModelState.Remove(err.PropertyName);
                context.ModelState.AddModelError(
                    err.PropertyName,
                    err.ErrorMessage
                );
            });
            context.Result = new BadRequestObjectResult(context.ModelState);
            return;
        }

        await next();
    }
}
