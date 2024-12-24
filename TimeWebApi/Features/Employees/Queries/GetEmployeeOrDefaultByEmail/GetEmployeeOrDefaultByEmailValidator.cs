namespace TimeWebApi.Features.Employees.Queries.GetEmployeeByEmail;

using FluentValidation;

public sealed class GetEmployeeOrDefaultByEmailValidator : AbstractValidator<GetEmployeeOrDefaultByEmailQuery>
{
    public GetEmployeeOrDefaultByEmailValidator()
    {
        RuleFor(query => query.Email)
            .NotEmpty()
                .WithMessage("Email can not be empty.")
            .EmailAddress()
                .WithMessage("Email is not in valid format.");
    }
}
