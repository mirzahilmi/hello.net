using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Hello.NET.ExceptionHandlers;

public class TimeOutExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        if (exception is not TimeoutException)
            return false;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = (int)HttpStatusCode.ServiceUnavailable,
                Type = exception.GetType().Name,
                Title = "A timeout occured",
                Detail = exception.Message,
                Instance =
                    $"{httpContext.Request.Method} {httpContext.Request.Path}",
            },
            cancellationToken: cancellationToken
        );
        return true;
    }
}
