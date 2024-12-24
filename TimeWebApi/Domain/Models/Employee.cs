namespace TimeWebApi.Domain.Models;

public sealed class Employee
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required int Id { get; set; }
    public required string LastName { get; set; }
}
