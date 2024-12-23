namespace TimeWebApi.Controllers.Employees.Requests;

public sealed class CreateEmployeeRequest
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}
