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
        if (exception is DataConflictException)
        {
            var details = new ProblemDetails
            {
                Type =
                    "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.10",
                Title = "Conflict",
                Status = StatusCodes.Status409Conflict,
                Detail = exception.Message,
                Instance =
                    $"{httpContext.Request.Method} {httpContext.Request.Path}",
            };

            httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
            await httpContext.Response.WriteAsJsonAsync(
                details,
                cancellationToken
            );

            return true;
        }

        return false;
    }
}
