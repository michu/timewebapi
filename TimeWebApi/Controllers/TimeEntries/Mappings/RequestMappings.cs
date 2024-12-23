namespace TimeWebApi.Controllers.TimeEntries.Mappings;

using TimeWebApi.Controllers.TimeEntries.Requests;
using TimeWebApi.Features.TimeEntries.Commands.CreateTimeEntry;
using TimeWebApi.Features.TimeEntries.Commands.UpdateTimeEntry;

public static class RequestMappings
{
    public static CreateTimeEntryCommand ToCommand(this CreateTimeEntryRequest request, int employeeId)
        => new CreateTimeEntryCommand
        {
            Date = request.Date,
            EmployeeId = employeeId,
            HoursWorked = request.HoursWorked
        };

    public static UpdateTimeEntryCommand ToCommand(this UpdateTimeEntryRequest request, int employeeId, int timeEntryId)
        => new UpdateTimeEntryCommand
        {
            Date = request.Date,
            EmployeeId = employeeId,
            HoursWorked = request.HoursWorked,
            Id = timeEntryId
        };
}
