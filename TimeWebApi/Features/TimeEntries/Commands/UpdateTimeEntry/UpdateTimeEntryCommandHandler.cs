namespace TimeWebApi.Features.TimeEntries.Commands.UpdateTimeEntry;

using MediatR;
using TimeWebApi.DAL.Employees.Interfaces;
using TimeWebApi.DAL.TimeEntries.Interfaces;
using TimeWebApi.Features.Common.Exceptions;
using TimeWebApi.Features.Common.Extensions;
using TimeWebApi.Features.Common.Messaging;

public sealed class UpdateTimeEntryCommandHandler : ICommandHandler<UpdateTimeEntryCommand, Unit>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITimeEntryRepository _timeEntryRepository;

    public UpdateTimeEntryCommandHandler(IEmployeeRepository employeeRepository, ITimeEntryRepository timeEntryRepository)
    {
        _employeeRepository = employeeRepository;
        _timeEntryRepository = timeEntryRepository;
    }

    public async Task<Unit> Handle(UpdateTimeEntryCommand command, CancellationToken cancellationToken)
    {
        await _employeeRepository.ThrowIfDoesNotExist(command.EmployeeId, cancellationToken);

        var timeEntry = await _timeEntryRepository.GetById(command.Id, cancellationToken);

        if (timeEntry == null)
        {
            throw new NotFoundException("Time Entry with given id does not exists.");
        }

        if (timeEntry.EmployeeId != command.EmployeeId)
        {
            throw new NotFoundException("Time Entry with given id is not owned by Employee with given employee-id.");
        }

        await _timeEntryRepository.ThrowIfExistsByEmployeeIdAndDate(command.EmployeeId, command.Date, command.Id, cancellationToken);

        await _timeEntryRepository.Update(command.ToEntity(), cancellationToken);

        return Unit.Value;
    }
}
