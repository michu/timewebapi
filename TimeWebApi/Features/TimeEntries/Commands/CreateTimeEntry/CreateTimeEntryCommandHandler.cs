namespace TimeWebApi.Features.TimeEntries.Commands.CreateTimeEntry;

using TimeWebApi.DAL.Employees.Interfaces;
using TimeWebApi.DAL.TimeEntries.Interfaces;
using TimeWebApi.Features.Common.Extensions;
using TimeWebApi.Features.Common.Messaging;

public sealed class CreateTimeEntryCommandHandler : ICommandHandler<CreateTimeEntryCommand, int>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITimeEntryRepository _timeEntryRepository;

    public CreateTimeEntryCommandHandler(IEmployeeRepository employeeRepository, ITimeEntryRepository repository)
    {
        _employeeRepository = employeeRepository;
        _timeEntryRepository = repository;
    }

    public async Task<int> Handle(CreateTimeEntryCommand command, CancellationToken cancellationToken)
    {
        await _employeeRepository.ThrowIfDoesNotExist(command.EmployeeId, cancellationToken);
        await _timeEntryRepository.ThrowIfExistsByEmployeeIdAndDate(command.EmployeeId, command.Date, cancellationToken);

        return await _timeEntryRepository.Add(command.ToEntity(), cancellationToken);
    }
}
