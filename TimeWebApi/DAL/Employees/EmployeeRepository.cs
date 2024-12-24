namespace TimeWebApi.DAL.Employees;

using Dapper;
using System.Data.Common;
using TimeWebApi.DAL.Employees.Interfaces;
using TimeWebApi.Domain.Models;

public sealed class EmployeeRepository : IEmployeeRepository
{
    private readonly DbConnection _connection;

    public EmployeeRepository(DbConnection connection)
    {
        _connection = connection;
    }

    public async Task<int> Add(Employee employee, CancellationToken cancellationToken)
        => await _connection.ExecuteScalarAsync<int>(new CommandDefinition(@"
INSERT INTO ""Employees"" (
    ""Email"",
    ""FirstName"",
    ""LastName""
) VALUES (
    @Email,
    @FirstName,
    @LastName
) RETURNING ""Id""",
            parameters: new { employee.Email, employee.FirstName, employee.LastName },
            cancellationToken: cancellationToken));

    public async Task Delete(int id, CancellationToken cancellationToken)
        => await _connection.ExecuteAsync(new CommandDefinition(@"
UPDATE ""Employees""
SET ""IsDeleted"" = true
WHERE ""Id"" = @Id",
            parameters: new { Id = id },
            cancellationToken: cancellationToken
        ));

    public async Task<bool> Exists(int id, CancellationToken cancellationToken)
        => await _connection.QueryFirstOrDefaultAsync<bool>(new CommandDefinition(@"
SELECT 1
FROM ""Employees""
WHERE ""Id"" = @Id
    AND ""IsDeleted"" = false",
        parameters: new { Id = id },
        cancellationToken: cancellationToken
    ));

    public async Task<bool> ExistsByEmail(string email, CancellationToken cancellationToken)
        => await _connection.QueryFirstOrDefaultAsync<bool>(new CommandDefinition(@"
SELECT 1
FROM ""Employees""
WHERE ""Email"" = @Email
    AND ""IsDeleted"" = false",
        parameters: new { Email = email },
        cancellationToken: cancellationToken
    ));

    public async Task<bool> ExistsByEmail(string email, int excludeId, CancellationToken cancellationToken)
        => await _connection.QueryFirstOrDefaultAsync<bool>(new CommandDefinition(@"
SELECT 1
FROM ""Employees""
WHERE ""Email"" = @Email
    AND ""Id"" <> @Id
    AND ""IsDeleted"" = false",
        parameters: new { Email = email, Id = excludeId },
        cancellationToken: cancellationToken
    ));

    public async Task<IEnumerable<Employee>> GetAll(CancellationToken cancellationToken)
        => await _connection.QueryAsync<Employee>(new CommandDefinition(@"
SELECT
    ""Email"",
    ""FirstName"",
    ""Id"",
    ""LastName""
FROM ""Employees""
WHERE ""IsDeleted"" = false
ORDER BY ""Id"" ASC",
            cancellationToken: cancellationToken));

    public async Task<Employee?> GetByEmail(string email, CancellationToken cancellationToken)
        => await _connection.QueryFirstOrDefaultAsync<Employee>(new CommandDefinition(@"
SELECT
    ""Email"",
    ""FirstName"",
    ""Id"",
    ""LastName""
FROM ""Employees""
WHERE ""Email"" = @Email
    AND ""IsDeleted"" = false",
            parameters: new { Email = email },
            cancellationToken: cancellationToken));

    public async Task<Employee?> GetById(int id, CancellationToken cancellationToken)
        => await _connection.QueryFirstOrDefaultAsync<Employee>(new CommandDefinition(@"
SELECT
    ""Email"",
    ""FirstName"",
    ""Id"",
    ""LastName""
FROM ""Employees""
WHERE ""Id"" = @Id
    AND ""IsDeleted"" = false",
            parameters: new { Id = id },
            cancellationToken: cancellationToken));

    public async Task Update(Employee employee, CancellationToken cancellationToken)
        => await _connection.ExecuteScalarAsync(new CommandDefinition(@"
UPDATE ""Employees"" SET
    ""Email"" = @Email,
    ""FirstName"" = @FirstName,
    ""LastName"" = @LastName
WHERE ""Id"" = @Id",
            parameters: new { employee.Email, employee.FirstName, employee.Id, employee.LastName },
            cancellationToken: cancellationToken));
}
