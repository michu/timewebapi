namespace TimeWebApi.Features.Employees.Commands.DeleteEmployee;

using Dapper;
using MediatR;
using Npgsql;
using TimeWebApi.Features.Common.Messaging;

public sealed class DeleteEmployeeCommandHandler : ICommandHandler<DeleteEmployeeCommand, Unit>
{
    private readonly NpgsqlConnection _connection;

    public DeleteEmployeeCommandHandler(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<Unit> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
    {
        await SoftDeleteEmployee(command, cancellationToken);

        return Unit.Value;
    }

    private async Task SoftDeleteEmployee(DeleteEmployeeCommand command, CancellationToken cancellationToken)
        => await _connection.ExecuteAsync(new CommandDefinition(@"
UPDATE ""Employees""
SET ""IsDeleted"" = true
WHERE ""Id"" = @Id",
            parameters: new { command.Id },
            cancellationToken: cancellationToken
        ));
}
