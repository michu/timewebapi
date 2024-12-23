namespace TimeWebApi.Controllers.TimeEntries.Responses;

public sealed class TimeEntryResponse
{
    public required DateOnly Date { get; set; }
    public required decimal HoursWorked { get; set; }
    public required int Id { get; set; }
}
