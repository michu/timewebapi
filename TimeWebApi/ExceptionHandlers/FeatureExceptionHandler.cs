namespace TimeWebApi.ExceptionHandlers;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TimeWebApi.Features.Common.Exceptions;

public sealed class FeatureExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is FeatureException)
        {
            await HandleFeatureException(context, (FeatureException)exception, cancellationToken);

            return true;
        }

        return false;
    }

    private static int GetStatusCode(FeatureException exception) =>
        exception switch
        {
            ConflictException => StatusCodes.Status409Conflict,
            NotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };

    private static string GetTitle(FeatureException exception) =>
        exception switch
        {
            ConflictException => "Conflict",
            NotFoundException => "Not Found",
            ValidationException => "Validation Error",
            _ => "Internal Server Error"
        };

    private static string GetType(FeatureException exception) =>
        exception switch
        {
            ConflictException => "https://tools.ietf.org/html/rfc9110#section-15.5.10",
            NotFoundException => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
            ValidationException => "https://tools.ietf.org/html/rfc9110#section-15.5.21",
            _ => "https://tools.ietf.org/html/rfc9110#section-15.6.1"
        };

    private static async Task HandleFeatureException(HttpContext context, FeatureException exception, CancellationToken cancellationToken)
    {
        context.Response.StatusCode = GetStatusCode(exception);

        var problemDetails = new ProblemDetails
        {
            Detail = exception.Message,
            Status = context.Response.StatusCode,
            Title = GetTitle(exception),
            Type = GetType(exception),
        };

        if (exception.Errors is not null)
        {
            problemDetails.Extensions["errors"] = exception.Errors;
        }

#if DEBUG
        problemDetails.Extensions["exception"] = exception.ToString();
        problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? context.TraceIdentifier;
#endif

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    }
}
