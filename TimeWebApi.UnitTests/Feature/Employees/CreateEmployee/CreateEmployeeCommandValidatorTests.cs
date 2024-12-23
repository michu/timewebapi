namespace TimeWebApi.UnitTests.Feature.Employees.CreateEmployee;

using FluentAssertions;
using FluentValidation.TestHelper;
using TimeWebApi.Features.Employees.Commands.CreateEmployee;

public sealed class CreateEmployeeCommandValidatorTests
{
    [Fact]
    public async Task Given_ValidCommand_Validate_ShouldPass()
    {
        var validator = new CreateEmployeeCommandValidator();
        var command = DefaultAndValidCommand;

        var result = await validator.TestValidateAsync(command);

        result.IsValid.Should().Be(true);
    }

    [Fact]
    public async Task Given_InvalidCommand_WithEmptyEmail_Validate_ShouldFail()
    {
        var validator = new CreateEmployeeCommandValidator();
        var command = DefaultAndValidCommand;
        command.Email = string.Empty;

        var result = await validator.TestValidateAsync(command);

        result.IsValid.Should().Be(false);
        result.ShouldHaveValidationErrorFor(command => command.Email)
            .WithErrorMessage("Email can not be empty.");
    }

    [Fact]
    public async Task Given_InvalidCommand_WithIncorrectEmail_Validate_ShouldFail()
    {
        var validator = new CreateEmployeeCommandValidator();
        var command = DefaultAndValidCommand;
        command.Email = "this-is-not-an-email-address";

        var result = await validator.TestValidateAsync(command);

        result.IsValid.Should().Be(false);
        result.ShouldHaveValidationErrorFor(command => command.Email)
            .WithErrorMessage("Email is not in valid format.");
    }

    private static CreateEmployeeCommand DefaultAndValidCommand
    {
        get => new CreateEmployeeCommand
        {
            Email = "admin@example.com",
            FirstName = "John",
            LastName = "Smith"
        };
    }

}
