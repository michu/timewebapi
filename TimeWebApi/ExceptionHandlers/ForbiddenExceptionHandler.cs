namespace TimeWebApi.ExceptionHandlers;

using Microsoft.AspNetCore.Diagnostics;
using TimeWebApi.Exceptions;

public sealed class ForbiddenExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        if (exception is ForbiddenException)
        {
            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status403Forbidden;

            return true;
        }

        return false;
    }
}
