namespace TimeWebApi.Features.TimeEntries.Queries.GetTimeEntries;

using TimeWebApi.DAL.Employees.Interfaces;
using TimeWebApi.DAL.TimeEntries.Interfaces;
using TimeWebApi.Features.Common.Extensions;
using TimeWebApi.Features.Common.Messaging;
using TimeWebApi.Features.TimeEntries.Mappings;
using TimeWebApi.Features.TimeEntries.Models;

public sealed class GetTimeEntriesQueryHandler : IQueryHandler<GetTimeEntriesQuery, IEnumerable<TimeEntryDto>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITimeEntryRepository _timeEntryRepository;

    public GetTimeEntriesQueryHandler(IEmployeeRepository employeeRepository, ITimeEntryRepository timeEntryRepository)
    {
        _employeeRepository = employeeRepository;
        _timeEntryRepository = timeEntryRepository;
    }

    public async Task<IEnumerable<TimeEntryDto>> Handle(GetTimeEntriesQuery query, CancellationToken cancellationToken)
    {
        await _employeeRepository.ThrowIfDoesNotExist(query.EmployeeId, cancellationToken);

        return (await _timeEntryRepository.GetByEmployeeId(query.EmployeeId, cancellationToken))
            .ToDtos();
    }
}
