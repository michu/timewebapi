namespace TimeWebApi.Controllers.Employees.Mappings;

using TimeWebApi.Controllers.Employees.Responses;
using TimeWebApi.Features.Employees.Models;

public static class ResponseMappings
{
    public static EmployeeResponse ToResponse(this EmployeeDto dto)
        => new EmployeeResponse
        {
            Email = dto.Email,
            FirstName = dto.FirstName,
            Id = dto.Id,
            LastName = dto.LastName,
        };

    public static IEnumerable<EmployeeResponse> ToResponse(this IEnumerable<EmployeeDto> dtos)
        => dtos.Select(ToResponse);
}
