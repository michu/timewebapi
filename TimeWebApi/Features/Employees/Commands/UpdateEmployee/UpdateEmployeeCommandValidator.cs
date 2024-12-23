namespace TimeWebApi.Features.Employees.Commands.UpdateEmployee;

using FluentValidation;

public sealed class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
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
