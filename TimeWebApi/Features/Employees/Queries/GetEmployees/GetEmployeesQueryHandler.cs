namespace TimeWebApi.Features.Employees.Queries.GetEmployees;

using TimeWebApi.DAL.Employees.Interfaces;
using TimeWebApi.Features.Common.Messaging;
using TimeWebApi.Features.Employees.Mappings;
using TimeWebApi.Features.Employees.Models;

public sealed class GetEmployeesQueryHandler : IQueryHandler<GetEmployeesQuery, IEnumerable<EmployeeDto>>
{
    private readonly IEmployeeRepository _repository;

    public GetEmployeesQueryHandler(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<EmployeeDto>> Handle(GetEmployeesQuery query, CancellationToken cancellationToken)
        => (await _repository.GetAll(cancellationToken))
            .ToDtos();
}
