namespace TimeWebApi.Features.Employees.Queries.GetEmployeeByEmail;

using System.Threading;
using System.Threading.Tasks;
using TimeWebApi.DAL.Employees.Interfaces;
using TimeWebApi.Features.Common.Messaging;
using TimeWebApi.Features.Employees.Mappings;
using TimeWebApi.Features.Employees.Models;

public class GetEmployeeOrDefaultByEmailQueryHandler : IQueryHandler<GetEmployeeOrDefaultByEmailQuery, EmployeeDto?>
{
    private readonly IEmployeeRepository _repository;

    public GetEmployeeOrDefaultByEmailQueryHandler(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<EmployeeDto?> Handle(GetEmployeeOrDefaultByEmailQuery query, CancellationToken cancellationToken)
        => (await _repository.GetByEmail(query.Email, cancellationToken))?.ToDto();
}
