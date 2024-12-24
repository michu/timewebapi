namespace TimeWebApi.Features.Employees.Queries.GetEmployeeByEmail;

using FluentValidation;

public sealed class GetEmployeeOrDefaultByEmailQueryValidator : AbstractValidator<GetEmployeeOrDefaultByEmailQuery>
{
    public GetEmployeeOrDefaultByEmailQueryValidator()
    {
        RuleFor(query => query.Email)
            .NotEmpty()
                .WithMessage("Email can not be empty.")
            .EmailAddress()
                .WithMessage("Email is not in valid format.");
    }
}
