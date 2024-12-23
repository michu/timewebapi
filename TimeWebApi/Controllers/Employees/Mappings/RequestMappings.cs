namespace TimeWebApi.Controllers.Employees.Mappings;

using TimeWebApi.Controllers.Employees.Requests;
using TimeWebApi.Features.Employees.Commands.CreateEmployee;
using TimeWebApi.Features.Employees.Commands.UpdateEmployee;

public static class RequestMappings
{
    public static CreateEmployeeCommand ToCommand(this CreateEmployeeRequest request)
        => new CreateEmployeeCommand
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

    public static UpdateEmployeeCommand ToCommand(this UpdateEmployeeRequest request, int employeeId)
        => new UpdateEmployeeCommand
        {
            Email = request.Email,
            Id = employeeId,
            FirstName = request.FirstName,
            LastName = request.LastName
        };
}
