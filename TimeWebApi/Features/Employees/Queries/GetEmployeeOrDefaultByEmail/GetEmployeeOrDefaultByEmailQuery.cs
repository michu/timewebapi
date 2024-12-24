namespace TimeWebApi.Features.Employees.Queries.GetEmployeeByEmail;

using TimeWebApi.Features.Common.Messaging;
using TimeWebApi.Features.Employees.Models;

public sealed class GetEmployeeOrDefaultByEmailQuery : IQuery<EmployeeDto?>
{
    public required string Email { get; set; }
}
