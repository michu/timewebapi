namespace TimeWebApi.Features.TimeEntries.Commands.CreateTimeEntry;

using TimeWebApi.Domain.Models;

public static class CreateTimeEntryCommandMappings
{
    public static TimeEntry ToEntity(this CreateTimeEntryCommand command)
        => new TimeEntry
        {
            Date = command.Date,
            EmployeeId = command.EmployeeId,
            HoursWorked = command.HoursWorked,
            Id = 0
        };
}
