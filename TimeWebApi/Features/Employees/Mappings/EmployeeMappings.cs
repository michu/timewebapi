namespace TimeWebApi.Features.Employees.Mappings;

using TimeWebApi.Domain.Models;
using TimeWebApi.Features.Employees.Models;

public static class EmployeeMappings
{
    public static EmployeeDto ToDto(this Employee employee)
        => new EmployeeDto
        {
            Email = employee.Email,
            FirstName = employee.FirstName,
            Id = employee.Id,
            LastName = employee.LastName,
        };

    public static IEnumerable<EmployeeDto> ToDtos(this IEnumerable<Employee> employees)
        => employees.Select(ToDto);
}
