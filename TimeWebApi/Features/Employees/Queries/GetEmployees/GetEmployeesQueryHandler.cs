namespace TimeWebApi.Features.Employees.Queries.GetEmployees;

using Dapper;
using Npgsql;
using TimeWebApi.Features.Common.Messaging;
using TimeWebApi.Features.Employees.Models;

public sealed class GetEmployeesQueryHandler : IQueryHandler<GetEmployeesQuery, IEnumerable<EmployeeDto>>
{
    private readonly NpgsqlConnection _connection;

    public GetEmployeesQueryHandler(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<EmployeeDto>> Handle(GetEmployeesQuery query, CancellationToken cancellationToken)
        => await GetEmployees(query, cancellationToken);

    private async Task<IEnumerable<EmployeeDto>> GetEmployees(GetEmployeesQuery query, CancellationToken cancellationToken)
        => await _connection.QueryAsync<EmployeeDto>(new CommandDefinition(@"
SELECT
    ""Email"",
    ""FirstName"",
    ""Id"",
    ""LastName""
FROM ""Employees""
WHERE ""IsDeleted"" = false
ORDER BY ""Id"" ASC",
            cancellationToken: cancellationToken));
}
