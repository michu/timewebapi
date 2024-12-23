namespace TimeWebApi.Features.Employees.Queries.GetEmployees;

using TimeWebApi.Features.Common.Messaging;
using TimeWebApi.Features.Employees.Models;

public sealed class GetEmployeesQuery : IQuery<IEnumerable<EmployeeDto>>
{
}
