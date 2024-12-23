namespace TimeWebApi.Features.TimeEntries.Queries.GetTimeEntries;

using Dapper;
using Npgsql;
using TimeWebApi.Features.Common.Extensions;
using TimeWebApi.Features.Common.Messaging;
using TimeWebApi.Features.TimeEntries.Models;

public sealed class GetTimeEntriesQueryHandler : IQueryHandler<GetTimeEntriesQuery, IEnumerable<TimeEntryDto>>
{
    private readonly NpgsqlConnection _connection;

    public GetTimeEntriesQueryHandler(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<TimeEntryDto>> Handle(GetTimeEntriesQuery query, CancellationToken cancellationToken)
    {
        await _connection.ThrowIfEmployeeDoesNotExist(query.EmployeeId, cancellationToken);

        return await GetTimeEntries(query, cancellationToken);
    }

    private async Task<IEnumerable<TimeEntryDto>> GetTimeEntries(GetTimeEntriesQuery query, CancellationToken cancellationToken)
        => await _connection.QueryAsync<TimeEntryDto>(new CommandDefinition(@"
SELECT
    ""Date"",
    ""HoursWorked"",
    ""Id""
FROM ""TimeEntries""
WHERE ""EmployeeId"" = @EmployeeId
ORDER BY ""Id"" ASC",
            cancellationToken: cancellationToken,
            parameters: new { query.EmployeeId }));
}
