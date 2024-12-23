namespace TimeWebApi.Features.Common.Extensions;

using Dapper;
using Npgsql;
using TimeWebApi.Features.Common.Exceptions;

public static partial class NpgsqlConnectionExtensions
{
    public static async Task<bool> ExistsTimeEntryByEmployeeAndDate(this NpgsqlConnection connection, int employeeId, DateOnly date, CancellationToken cancellationToken)
            => await connection.QueryFirstOrDefaultAsync<bool>(new CommandDefinition(@"
SELECT 1
FROM ""TimeEntries""
WHERE ""Date"" = @Date
    AND ""EmployeeId"" = @EmployeeId",
            parameters: new { Date = date, EmployeeId = employeeId },
            cancellationToken: cancellationToken
        ));

    public static async Task<bool> IsTimeEntryOwnedByEmployee(this NpgsqlConnection connection, int id, int employeeId, CancellationToken cancellationToken)
            => await connection.QueryFirstOrDefaultAsync<bool>(new CommandDefinition(@"
SELECT 1
FROM ""TimeEntries"" AS ""TE""
INNER JOIN ""Employees"" AS ""E"" ON ""TE"".""EmployeeId"" = ""E"".""Id""
WHERE ""TE"".""Id"" = @Id
    AND ""E"".""Id"" = @EmployeeId
    AND ""E"".""IsDeleted"" = false",
            parameters: new { EmployeeId = employeeId, Id = id },
            cancellationToken: cancellationToken
        ));

    public static async Task ThrowIfTimeEntryWithGivenEmployeeAndDateExists(this NpgsqlConnection connection, int employeeId, DateOnly date, CancellationToken cancellationToken)
    {
        var exists = await connection.ExistsTimeEntryByEmployeeAndDate(employeeId, date, cancellationToken);

        if (exists)
        {
            throw new ConflictException("Time Entry with given date already exists.");
        }
    }

    public static async Task ThrowIfTimeEntryIsNotOwnedByEmployee(this NpgsqlConnection connection, int id, int employeeId, CancellationToken cancellationToken)
    {
        var isOwned = await connection.IsTimeEntryOwnedByEmployee(id, employeeId, cancellationToken);

        if (!isOwned)
        {
            throw new NotFoundException("Time Entry with given id is not owned by Employee with given employee-id.");
        }
    }
}
