﻿namespace TimeWebApi.Middlewares;

using System.Security.Claims;
using TimeWebApi.Exceptions;
using TimeWebApi.Resources;

public sealed class EmployeeBasedAccessMiddleware
{
    private readonly RequestDelegate _next;

    public EmployeeBasedAccessMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.IsInRole(StaticData.Roles.Admin))
        {
            await _next(context);

            return;
        }

        /* Warning:
         * This is not a good solution for checking whether or not user has access to a specific resource.
         * The main vulnerability is that it require each link to contain employee-id related route-value, when link has no such value then user will access the resource.
         * I did it like that just for simplicity.
         */

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

        var claimsIdentity = context.User.Identity as ClaimsIdentity;

        if (claimsIdentity == null)
        {
            throw new ForbiddenException("Access to resource is not granted for currently logged in user.");
        }

        var claimsEmployeeId = claimsIdentity.Claims.FirstOrDefault(claim => claim.Type == StaticData.Claims.EmployeeId);

        if (claimsEmployeeId == null || claimsEmployeeId.Value != employeeIdByResourceInt.ToString())
        {
            throw new ForbiddenException("Access to resource is not granted for currently logged in user.");
        }

        await _next(context);
    }
}
