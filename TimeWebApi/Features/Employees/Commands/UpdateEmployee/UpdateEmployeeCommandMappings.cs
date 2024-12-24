namespace TimeWebApi.Features.Employees.Commands.UpdateEmployee;

using TimeWebApi.Domain.Models;

public static class UpdateEmployeeCommandMappings
{
    public static Employee ToEntity(this UpdateEmployeeCommand command)
        => new Employee
        {
            Email = command.Email,
            FirstName = command.FirstName,
            Id = command.Id,
            LastName = command.LastName
        };
}
