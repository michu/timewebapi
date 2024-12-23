namespace TimeWebApi.Features.Employees.Queries.GetEmployee;

using TimeWebApi.Features.Common.Messaging;
using TimeWebApi.Features.Employees.Models;

public sealed class GetEmployeeQuery : IQuery<EmployeeDto>
{
    public int Id { get; set; }
}
