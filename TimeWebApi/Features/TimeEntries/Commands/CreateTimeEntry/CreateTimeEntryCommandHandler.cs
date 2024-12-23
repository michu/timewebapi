namespace TimeWebApi.Features.TimeEntries.Commands.CreateTimeEntry;

using Dapper;
using MediatR;
using Npgsql;
using TimeWebApi.Features.Common.Extensions;
using TimeWebApi.Features.Common.Messaging;

public sealed class CreateTimeEntryCommandHandler : ICommandHandler<CreateTimeEntryCommand, Unit>
{
    private readonly NpgsqlConnection _connection;

    public CreateTimeEntryCommandHandler(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<Unit> Handle(CreateTimeEntryCommand command, CancellationToken cancellationToken)
    {
        await _connection.ThrowIfEmployeeDoesNotExist(command.EmployeeId, cancellationToken);
        await _connection.ThrowIfTimeEntryWithGivenEmployeeAndDateExists(command.EmployeeId, command.Date, cancellationToken);

        await CreateTimeEntry(command, cancellationToken);

        return Unit.Value;
    }

    private async Task CreateTimeEntry(CreateTimeEntryCommand command, CancellationToken cancellationToken)
        => await _connection.ExecuteScalarAsync(new CommandDefinition(@"
INSERT INTO ""TimeEntries"" (
    ""Date"",
    ""EmployeeId"",
    ""HoursWorked""
) VALUES (
    @Date,
    @EmployeeId,
    @HoursWorked
)",
            parameters: new { command.Date, command.EmployeeId, command.HoursWorked },
            cancellationToken: cancellationToken
        ));
}
