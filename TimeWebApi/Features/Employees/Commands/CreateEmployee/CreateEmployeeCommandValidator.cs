namespace TimeWebApi.Features.Employees.Commands.CreateEmployee;

using FluentValidation;

public sealed class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty()
                .WithMessage("Email can not be empty.")
            .EmailAddress()
                .WithMessage("Email is not in valid format.");

        RuleFor(command => command.FirstName)
            .NotEmpty()
                .WithMessage("First name can not be empty.");

        RuleFor(command => command.LastName)
            .NotEmpty()
                .WithMessage("Last name can not be empty.");
    }
}
