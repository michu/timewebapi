namespace TimeWebApi.Features.TimeEntries.Commands.DeleteTimeEntry;

using MediatR;
using TimeWebApi.Features.Common.Messaging;

public sealed class DeleteTimeEntryCommand : ITransactionCommand<Unit>
{
    public required int EmployeeId { get; set; }
    public required int Id { get; set; }
}
