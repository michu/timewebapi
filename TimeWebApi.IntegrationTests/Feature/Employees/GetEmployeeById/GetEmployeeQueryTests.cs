namespace TimeWebApi.IntegrationTests.Feature.Employees.GetEmployeeById;

using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using TimeWebApi.Features.Common.Exceptions;
using TimeWebApi.Features.Employees.Queries.GetEmployee;

public sealed class GetEmployeeQueryTests : IntegrationTestBase
{
    [Fact]
    public async Task Given_Query_WithExistentEmployeeId_Handle_ShouldReturnEmployee()
    {
        var connection = Scope.ServiceProvider.GetRequiredService<NpgsqlConnection>();
        var mediator = Scope.ServiceProvider.GetRequiredService<IMediator>();

        await AddEmployeeToDatabase(connection, new EmployeeData(1, "admin@example.com", "John", "Smith"));
        await AddEmployeeToDatabase(connection, new EmployeeData(2, "jane@example.com", "Jane", "Brown"));
        await AddEmployeeToDatabase(connection, new EmployeeData(3, "adam@example.com", "Adam", "Davis"));

        var employeeDto = await mediator.Send(new GetEmployeeQuery
        {
            Id = 2
        });

        employeeDto.Email.Should().Be("jane@example.com");
        employeeDto.FirstName.Should().Be("Jane");
        employeeDto.Id.Should().Be(2);
        employeeDto.LastName.Should().Be("Brown");
    }

    [Fact]
    public async Task Given_Query_WithNonExistentEmployeeId_Handle_ShouldThrowNotFoundException()
    {
        var connection = Scope.ServiceProvider.GetRequiredService<NpgsqlConnection>();
        var mediator = Scope.ServiceProvider.GetRequiredService<IMediator>();

        await AddEmployeeToDatabase(connection, new EmployeeData(1, "admin@example.com", "John", "Smith"));
        await AddEmployeeToDatabase(connection, new EmployeeData(2, "jane@example.com", "Jane", "Brown"));

        var send = () => mediator.Send(new GetEmployeeQuery
        {
            Id = 3
        });

        var exception = (await send.Should().ThrowAsync<NotFoundException>()).And;
        exception.Errors.Should().BeNull();
        exception.Message.Should().Be("Employee with given id does not exist.");
    }
}
