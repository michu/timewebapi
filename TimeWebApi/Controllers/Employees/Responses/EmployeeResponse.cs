namespace TimeWebApi.Controllers.Employees.Responses;

public sealed class EmployeeResponse
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required int Id { get; set; }
    public required string LastName { get; set; }
}
