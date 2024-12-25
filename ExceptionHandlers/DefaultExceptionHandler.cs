using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Hello.NET.ExceptionHandlers;

public class DefaultExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Type = exception.GetType().Name,
                Title = "Internal Server Error",
                Detail = exception.Message,
                Instance =
                    $"{httpContext.Request.Method} {httpContext.Request.Path}",
            },
            cancellationToken: cancellationToken
        );
        return true;
    }
}
