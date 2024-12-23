namespace TimeWebApi.Features.Employees.Models;

public sealed class EmployeeDto
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required int Id { get; set; }
    public required string LastName { get; set; }
}
