namespace TimeWebApi.Features.TimeEntries.Models;

public sealed class TimeEntryDto
{
    public required DateOnly Date {  get; set; }
    public required decimal HoursWorked { get; set; }
    public required int Id { get; set; }
}
