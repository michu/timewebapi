using System.Net;

namespace TimeWebApi.Middlewares;

public sealed class ForbiddenMiddleware
{
    readonly RequestDelegate _next;

    public ForbiddenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        await _next(context);

        if (!context.Response.HasStarted && context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
}
