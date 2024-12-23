namespace TimeWebApi.Features.TimeEntries.Commands.UpdateTimeEntry;

using Dapper;
using MediatR;
using Npgsql;
using TimeWebApi.Features.Common.Extensions;
using TimeWebApi.Features.Common.Messaging;

public sealed class UpdateTimeEntryCommandHandler : ICommandHandler<UpdateTimeEntryCommand, Unit>
{
    private readonly NpgsqlConnection _connection;

    public UpdateTimeEntryCommandHandler(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<Unit> Handle(UpdateTimeEntryCommand command, CancellationToken cancellationToken)
    {
        await _connection.ThrowIfEmployeeDoesNotExist(command.EmployeeId, cancellationToken);
        await _connection.ThrowIfTimeEntryIsNotOwnedByEmployee(command.Id, command.EmployeeId, cancellationToken);

        await UpdateEmployee(command, cancellationToken);

        return Unit.Value;
    }

    private async Task UpdateEmployee(UpdateTimeEntryCommand command, CancellationToken cancellationToken)
        => await _connection.ExecuteScalarAsync(new CommandDefinition(@"
UPDATE ""TimeEntries"" SET
    ""Date"" = @Date,
    ""HoursWorked"" = @HoursWorked
WHERE ""Id"" = @Id",
            parameters: new { command.Date, command.HoursWorked, command.Id
},
            cancellationToken: cancellationToken
        ));

}
