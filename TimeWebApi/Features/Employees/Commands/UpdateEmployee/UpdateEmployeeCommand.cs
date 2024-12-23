namespace TimeWebApi.Features.Employees.Commands.UpdateEmployee;

using MediatR;
using TimeWebApi.Features.Common.Messaging;

public sealed class UpdateEmployeeCommand : ITransactionCommand<Unit>
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required int Id { get; set; }
    public required string LastName { get; set; }
}
