namespace TimeWebApi.Features.Employees.Queries.GetEmployee;

using Dapper;
using Npgsql;
using TimeWebApi.Features.Common.Exceptions;
using TimeWebApi.Features.Common.Messaging;
using TimeWebApi.Features.Employees.Models;

public sealed class GetEmployeeQueryHandler : IQueryHandler<GetEmployeeQuery, EmployeeDto>
{
    private readonly NpgsqlConnection _connection;

    public GetEmployeeQueryHandler(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<EmployeeDto> Handle(GetEmployeeQuery query, CancellationToken cancellationToken)
    {
        var employee = await GetEmployee(query, cancellationToken);

        if (employee is null)
        {
            throw new NotFoundException("Employee with given id does not exist.");
        }

        return employee;
    }

    private async Task<EmployeeDto?> GetEmployee(GetEmployeeQuery query, CancellationToken cancellationToken)
        => await _connection.QueryFirstOrDefaultAsync<EmployeeDto>(new CommandDefinition(@"
SELECT
    ""Email"",
    ""FirstName"",
    ""Id"",
    ""LastName""
FROM ""Employees""
WHERE ""Id"" = @Id
    AND ""IsDeleted"" = false",
            cancellationToken: cancellationToken,
            parameters: new { query.Id }));
}
