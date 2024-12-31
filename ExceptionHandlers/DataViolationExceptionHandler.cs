using System.Net;
using Hello.NET.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Hello.NET.ExceptionHandlers;

public sealed class DataViolationExceptionHandler : IExceptionHandler
{
    // TODO: find way todo guard clause
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        if (exception is DataDuplicateException)
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

        return false;
    }
}
