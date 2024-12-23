namespace TimeWebApi.Controllers.TimeEntries.Requests;

public sealed class CreateTimeEntryRequest
{
    public required DateOnly Date { get; set; }
    public required decimal HoursWorked { get; set; }
}
