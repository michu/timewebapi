namespace TimeWebApi.Features.Employees.Commands.CreateEmployee;

using TimeWebApi.Features.Common.Messaging;

public sealed class CreateEmployeeCommand : ITransactionCommand<int>
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}
