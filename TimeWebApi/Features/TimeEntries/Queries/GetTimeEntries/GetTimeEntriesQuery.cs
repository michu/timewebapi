namespace TimeWebApi.Features.TimeEntries.Queries.GetTimeEntries;

using TimeWebApi.Features.Common.Messaging;
using TimeWebApi.Features.TimeEntries.Models;

public sealed class GetTimeEntriesQuery : IQuery<IEnumerable<TimeEntryDto>>
{
    public required int EmployeeId { get; set; }
}
