namespace TimeWebApi.Middlewares;

using Dapper;
using Npgsql;
using TimeWebApi.Exceptions;
using TimeWebApi.Resources;

public sealed class EmployeeBasedAccessMiddleware
{
    private readonly NpgsqlDataSource _dataSource;
    private readonly RequestDelegate _next;

    public EmployeeBasedAccessMiddleware(NpgsqlDataSource dataSource, RequestDelegate next)
    {
        _dataSource = dataSource;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.IsInRole(StaticData.Roles.Admin))
        {
            await _next(context);

            return;
        }

        var routeValues = context.Request.RouteValues;

        if (!routeValues.TryGetValue(StaticData.RouteValueKeys.EmployeeId, out var employeeIdByResource))
        {
            await _next(context);

            return;
        }

        if (employeeIdByResource == null)
        {
            await _next(context);

            return;
        }

        if (!int.TryParse(employeeIdByResource.ToString(), out var employeeIdByResourceInt))
        {
            await _next(context);

            return;
        }

        using var connection = await _dataSource.OpenConnectionAsync();

        var employeeIdByEmail = await (context.User.Identity?.Name is not null
            ? connection.QueryFirstOrDefaultAsync<int?>(new CommandDefinition(@"
SELECT ""Id""
FROM ""Employees""
WHERE ""Email"" = @Email
AND ""IsDeleted"" = false",
    parameters: new { Email = context.User.Identity.Name }
))
            : Task.FromResult<int?>(null));

        if (employeeIdByResourceInt != employeeIdByEmail)
        {
            throw new ForbiddenException("Access to resource is not granted for currently logged in user.");
        }

        await _next(context);
    }
}
