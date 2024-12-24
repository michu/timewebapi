namespace TimeWebApi.Features.TimeEntries.Commands.CreateTimeEntry;

using TimeWebApi.Features.Common.Messaging;

public sealed class CreateTimeEntryCommand : ITransactionCommand<int>
{
    public required DateOnly Date { get; set; }
    public required int EmployeeId { get; set; }
    public required decimal HoursWorked { get; set; }
}
