namespace TimeWebApi.Features.TimeEntries.Commands.UpdateTimeEntry;

using MediatR;
using TimeWebApi.Features.Common.Messaging;

public sealed class UpdateTimeEntryCommand : ITransactionCommand<Unit>
{
    public required DateOnly Date { get; set; }
    public required int EmployeeId { get; set; }
    public required decimal HoursWorked { get; set; }
    public required int Id { get; set; }
}
