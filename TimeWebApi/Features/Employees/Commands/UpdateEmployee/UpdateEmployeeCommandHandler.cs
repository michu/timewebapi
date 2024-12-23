namespace TimeWebApi.Features.Employees.Commands.UpdateEmployee;

using Dapper;
using MediatR;
using Npgsql;
using TimeWebApi.Features.Common.Extensions;
using TimeWebApi.Features.Common.Messaging;

public sealed class UpdateEmployeeCommandHandler : ICommandHandler<UpdateEmployeeCommand, Unit>
{
    private readonly NpgsqlConnection _connection;

    public UpdateEmployeeCommandHandler(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<Unit> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        await _connection.ThrowIfEmployeeDoesNotExist(command.Id, cancellationToken);
        await _connection.ThrowIfEmployeeWithGivenEmailAlreadyExists(command.Email, command.Id, cancellationToken);

        await UpdateEmployee(command, cancellationToken);

        return Unit.Value;
    }

    private async Task UpdateEmployee(UpdateEmployeeCommand command, CancellationToken cancellationToken)
        => await _connection.ExecuteScalarAsync(new CommandDefinition(@"
UPDATE ""Employees"" SET
    ""Email"" = @Email,
    ""FirstName"" = @FirstName,
    ""LastName"" = @LastName
WHERE ""Id"" = @Id",
            parameters: new { command.Email, command.FirstName, command.Id, command.LastName },
            cancellationToken: cancellationToken
        ));
}
