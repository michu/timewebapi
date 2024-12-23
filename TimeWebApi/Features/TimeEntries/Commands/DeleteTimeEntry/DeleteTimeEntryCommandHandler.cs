namespace TimeWebApi.Features.TimeEntries.Commands.DeleteTimeEntry;

using Dapper;
using MediatR;
using Npgsql;
using TimeWebApi.Features.Common.Extensions;
using TimeWebApi.Features.Common.Messaging;

public sealed class DeleteTimeEntryCommandHandler : ICommandHandler<DeleteTimeEntryCommand, Unit>
{
    private readonly NpgsqlConnection _connection;

    public DeleteTimeEntryCommandHandler(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<Unit> Handle(DeleteTimeEntryCommand command, CancellationToken cancellationToken)
    {
        await _connection.ThrowIfEmployeeDoesNotExist(command.EmployeeId, cancellationToken);
        await _connection.ThrowIfTimeEntryIsNotOwnedByEmployee(command.Id, command.EmployeeId, cancellationToken);

        await DeleteTimeEntry(command, cancellationToken);

        return Unit.Value;
    }

    private async Task DeleteTimeEntry(DeleteTimeEntryCommand command, CancellationToken cancellationToken)
        => await _connection.ExecuteAsync(new CommandDefinition(@"
DELETE FROM ""TimeEntries""
WHERE ""Id"" = @Id",
            parameters: new { command.Id },
            cancellationToken: cancellationToken
        ));

}
