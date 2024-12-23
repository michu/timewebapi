namespace TimeWebApi.Features.Employees.Commands.DeleteEmployee;

using MediatR;
using TimeWebApi.Features.Common.Messaging;

public sealed class DeleteEmployeeCommand : ITransactionCommand<Unit>
{
    public required int Id { get; set; }
}
