namespace TimeWebApi.Controllers.TimeEntries.Requests;

public sealed class UpdateTimeEntryRequest
{
    public required DateOnly Date { get; set; }
    public required decimal HoursWorked { get; set; }
}
