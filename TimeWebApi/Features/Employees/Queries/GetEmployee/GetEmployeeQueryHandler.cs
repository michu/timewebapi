namespace TimeWebApi.Features.Employees.Queries.GetEmployee;

using TimeWebApi.DAL.Employees.Interfaces;
using TimeWebApi.Features.Common.Exceptions;
using TimeWebApi.Features.Common.Messaging;
using TimeWebApi.Features.Employees.Mappings;
using TimeWebApi.Features.Employees.Models;

public sealed class GetEmployeeQueryHandler : IQueryHandler<GetEmployeeQuery, EmployeeDto>
{
    private readonly IEmployeeRepository _repository;

    public GetEmployeeQueryHandler(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<EmployeeDto> Handle(GetEmployeeQuery query, CancellationToken cancellationToken)
    {
        var employee = await _repository.GetById(query.Id, cancellationToken);

        if (employee == null)
        {
            throw new NotFoundException("Employee with given id does not exist.");
        }

        return employee.ToDto();
    }
}
