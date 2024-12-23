namespace TimeWebApi.Features.Employees.Commands.CreateEmployee;

using Dapper;
using Npgsql;
using TimeWebApi.Features.Common.Extensions;
using TimeWebApi.Features.Common.Messaging;

public sealed class CreateEmployeeCommandHandler : ICommandHandler<CreateEmployeeCommand, int>
{
    private readonly NpgsqlConnection _connection;

    public CreateEmployeeCommandHandler(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<int> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        await _connection.ThrowIfEmployeeWithGivenEmailAlreadyExists(command.Email, cancellationToken);

        return await CreateEmployee(command, cancellationToken);
    }

    private async Task<int> CreateEmployee(CreateEmployeeCommand command, CancellationToken cancellationToken)
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
            parameters: new { command.Email, command.FirstName, command.LastName },
            cancellationToken: cancellationToken
        ));
}
