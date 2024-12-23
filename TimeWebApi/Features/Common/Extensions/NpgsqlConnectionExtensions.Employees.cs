namespace TimeWebApi.Features.Common.Extensions;

using Dapper;
using Npgsql;
using TimeWebApi.Features.Common.Exceptions;

public static partial class NpgsqlConnectionExtensions
{
    public static async Task<bool> ExistsEmployeeByEmail(this NpgsqlConnection connection, string email, CancellationToken cancellationToken)
        => await connection.QueryFirstOrDefaultAsync<bool>(new CommandDefinition(@"
SELECT 1
FROM ""Employees""
WHERE ""Email"" = @Email
    AND ""IsDeleted"" = false",
        parameters: new { Email = email },
        cancellationToken: cancellationToken
    ));

    public static async Task<bool> ExistsEmployeeByEmail(this NpgsqlConnection connection, string email, int excludeId, CancellationToken cancellationToken)
        => await connection.QueryFirstOrDefaultAsync<bool>(new CommandDefinition(@"
SELECT 1
FROM ""Employees""
WHERE ""Email"" = @Email
    AND ""Id"" <> @Id
    AND ""IsDeleted"" = false",
        parameters: new { Email = email, Id = excludeId },
        cancellationToken: cancellationToken
    ));

    public static async Task<bool> ExistsEmployee(this NpgsqlConnection connection, int id, CancellationToken cancellationToken)
        => await connection.QueryFirstOrDefaultAsync<bool>(new CommandDefinition(@"
SELECT 1
FROM ""Employees""
WHERE ""Id"" = @Id
    AND ""IsDeleted"" = false",
        parameters: new { Id = id },
        cancellationToken: cancellationToken
    ));

    public static async Task ThrowIfEmployeeDoesNotExist(this NpgsqlConnection connection, int id, CancellationToken cancellationToken)
    {
        var exists = await connection.ExistsEmployee(id, cancellationToken);

        if (!exists)
        {
            throw new NotFoundException("Employee with given id does not exists.");
        }
    }

    public static async Task ThrowIfEmployeeWithGivenEmailAlreadyExists(this NpgsqlConnection connection, string email, CancellationToken cancellationToken)
    {
        var exists = await connection.ExistsEmployeeByEmail(email, cancellationToken);

        if (exists)
        {
            throw new ConflictException("Employee with given email already exists.");
        }
    }

    public static async Task ThrowIfEmployeeWithGivenEmailAlreadyExists(this NpgsqlConnection connection, string email, int excludeId, CancellationToken cancellationToken)
    {
        var exists = await connection.ExistsEmployeeByEmail(email, excludeId, cancellationToken);

        if (exists)
        {
            throw new ConflictException("Employee with given email already exists.");
        }
    }
}
