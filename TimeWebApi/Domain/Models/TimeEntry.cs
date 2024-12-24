namespace TimeWebApi.Domain.Models;

public sealed class TimeEntry
{
    public required DateOnly Date { get; set; }
    public required decimal HoursWorked { get; set; }
    public required int EmployeeId { get; set; }
    public required int Id { get; set; }
}
