namespace TimeWebApi.Features.TimeEntries.Commands.DeleteTimeEntry;

using MediatR;
using TimeWebApi.DAL.Employees.Interfaces;
using TimeWebApi.DAL.TimeEntries.Interfaces;
using TimeWebApi.Features.Common.Exceptions;
using TimeWebApi.Features.Common.Extensions;
using TimeWebApi.Features.Common.Messaging;

public sealed class DeleteTimeEntryCommandHandler : ICommandHandler<DeleteTimeEntryCommand, Unit>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITimeEntryRepository _timeEntryRepository;

    public DeleteTimeEntryCommandHandler(IEmployeeRepository employeeRepository, ITimeEntryRepository timeEntryRepository)
    {
        _employeeRepository = employeeRepository;
        _timeEntryRepository = timeEntryRepository;
    }

    public async Task<Unit> Handle(DeleteTimeEntryCommand command, CancellationToken cancellationToken)
    {
        await _employeeRepository.ThrowIfDoesNotExist(command.EmployeeId, cancellationToken);

        var timeEntry = await _timeEntryRepository.GetById(command.Id, cancellationToken);

        if (timeEntry == null)
        {
            return Unit.Value;
        }

        if (timeEntry.EmployeeId != command.EmployeeId)
        {
            throw new NotFoundException("Time Entry with given id is not owned by Employee with given employee-id.");
        }

        await _timeEntryRepository.Delete(command.Id, cancellationToken);

        return Unit.Value;
    }
}
