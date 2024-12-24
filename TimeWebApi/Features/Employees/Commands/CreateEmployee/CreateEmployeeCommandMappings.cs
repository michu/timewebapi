namespace TimeWebApi.Features.Employees.Commands.CreateEmployee;

using TimeWebApi.Domain.Models;

public static class CreateEmployeeCommandMappings
{
    public static Employee ToEntity(this CreateEmployeeCommand command)
        => new Employee
        {
            Email = command.Email,
            FirstName = command.FirstName,
            Id = 0,
            LastName = command.LastName
        };
}
