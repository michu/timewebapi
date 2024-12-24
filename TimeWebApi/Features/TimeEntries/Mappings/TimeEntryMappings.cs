namespace TimeWebApi.Features.TimeEntries.Mappings;

using TimeWebApi.Domain.Models;
using TimeWebApi.Features.TimeEntries.Models;

public static class TimeEntryMappings
{
    public static TimeEntryDto ToDto(this TimeEntry timeEntry)
        => new TimeEntryDto
        {
            Date = timeEntry.Date,
            HoursWorked = timeEntry.HoursWorked,
            Id = timeEntry.Id
        };

    public static IEnumerable<TimeEntryDto> ToDtos(this IEnumerable<TimeEntry> timeEntries)
        => timeEntries.Select(ToDto);
}
