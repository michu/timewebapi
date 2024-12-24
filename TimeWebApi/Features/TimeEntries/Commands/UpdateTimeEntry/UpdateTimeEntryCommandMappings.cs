namespace TimeWebApi.Features.TimeEntries.Commands.UpdateTimeEntry;

using TimeWebApi.Domain.Models;

public static class UpdateTimeEntryCommandMappings
{
    public static TimeEntry ToEntity(this UpdateTimeEntryCommand command)
        => new TimeEntry
        {
            Date = command.Date,
            EmployeeId = command.EmployeeId,
            HoursWorked = command.HoursWorked,
            Id = command.Id
        };
}
