namespace TimeWebApi.Controllers.TimeEntries.Mappings;

using TimeWebApi.Controllers.TimeEntries.Responses;
using TimeWebApi.Features.TimeEntries.Models;

public static class ResponseMappings
{
    public static IEnumerable<TimeEntryResponse> ToResponse(this IEnumerable<TimeEntryDto> dtos)
        => dtos.Select(dto => new TimeEntryResponse
        {
            Date = dto.Date,
            HoursWorked = dto.HoursWorked,
            Id = dto.Id
        });
}
