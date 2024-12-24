namespace TimeWebApi.DAL.TimeEntries;

using Dapper;
using System;
using System.Data.Common;
using TimeWebApi.DAL.TimeEntries.Interfaces;
using TimeWebApi.Domain.Models;

public sealed class TimeEntryRepository : ITimeEntryRepository
{
    private readonly DbConnection _connection;

    public TimeEntryRepository(DbConnection connection)
    {
        _connection = connection;
    }

    public async Task<int> Add(TimeEntry timeEntry, CancellationToken cancellationToken)
        => await _connection.ExecuteScalarAsync<int>(new CommandDefinition(@"
INSERT INTO ""TimeEntries"" (
    ""Date"",
    ""EmployeeId"",
    ""HoursWorked""
) VALUES (
    @Date,
    @EmployeeId,
    @HoursWorked
) RETURNING ""Id""",
        parameters: new { timeEntry.Date, timeEntry.EmployeeId, timeEntry.HoursWorked },
        cancellationToken: cancellationToken));

    public async Task Delete(int id, CancellationToken cancellationToken)
        => await _connection.ExecuteAsync(new CommandDefinition(@"
DELETE FROM ""TimeEntries""
WHERE ""Id"" = @Id",
            parameters: new { Id = id },
            cancellationToken: cancellationToken
        ));

    public async Task<bool> Exists(int id, CancellationToken cancellationToken)
        => await _connection.QueryFirstOrDefaultAsync<bool>(new CommandDefinition(@"
SELECT 1
FROM ""TimeEntries""
WHERE ""Id"" = @Id",
        parameters: new { Id = id },
        cancellationToken: cancellationToken
    ));

    public async Task<bool> ExistsByEmployeeIdAndDate(int employeeId, DateOnly date, CancellationToken cancellationToken)
        => await _connection.QueryFirstOrDefaultAsync<bool>(new CommandDefinition(@"
SELECT 1
FROM ""TimeEntries""
WHERE ""Date"" = @Date
    AND ""EmployeeId"" = @EmployeeId",
            parameters: new { Date = date, EmployeeId = employeeId },
            cancellationToken: cancellationToken
        ));

    public async Task<bool> ExistsByEmployeeIdAndDate(int employeeId, DateOnly date, int excludeId, CancellationToken cancellationToken)
        => await _connection.QueryFirstOrDefaultAsync<bool>(new CommandDefinition(@"
SELECT 1
FROM ""TimeEntries""
WHERE ""Date"" = @Date
    AND ""EmployeeId"" = @EmployeeId
    AND ""Id"" <> @Id",
            parameters: new { Date = date, EmployeeId = employeeId, Id = excludeId },
            cancellationToken: cancellationToken
        ));

    public async Task<IEnumerable<TimeEntry>> GetByEmployeeId(int employeeId, CancellationToken cancellationToken)
        => await _connection.QueryAsync<TimeEntry>(new CommandDefinition(@"
SELECT
    ""Date"",
    ""EmployeeId"",
    ""HoursWorked"",
    ""Id""
FROM ""TimeEntries""
WHERE ""EmployeeId"" = @EmployeeId
ORDER BY ""Id"" ASC",
            cancellationToken: cancellationToken,
            parameters: new { EmployeeId = employeeId }));

    public async Task<TimeEntry?> GetById(int id, CancellationToken cancellationToken)
        => await _connection.QueryFirstOrDefaultAsync<TimeEntry>(new CommandDefinition(@"
SELECT
    ""Date"",
    ""EmployeeId"",
    ""HoursWorked"",
    ""Id""
FROM ""TimeEntries""
WHERE ""Id"" = @Id
ORDER BY ""Id"" ASC",
            cancellationToken: cancellationToken,
            parameters: new { Id = id }));

    public async Task Update(TimeEntry timeEntry, CancellationToken cancellationToken)
        => await _connection.ExecuteScalarAsync(new CommandDefinition(@"
UPDATE ""TimeEntries"" SET
    ""Date"" = @Date,
    ""HoursWorked"" = @HoursWorked
WHERE ""Id"" = @Id
    AND ""EmployeeId"" = @EmployeeId",
            parameters: new { timeEntry.Date, timeEntry.HoursWorked, timeEntry.EmployeeId, timeEntry.Id },
            cancellationToken: cancellationToken
        ));
}
