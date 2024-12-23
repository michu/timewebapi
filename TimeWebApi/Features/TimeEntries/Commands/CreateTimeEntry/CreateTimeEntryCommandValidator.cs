namespace TimeWebApi.Features.TimeEntries.Commands.CreateTimeEntry;

using FluentValidation;

public sealed class CreateTimeEntryCommandValidator : AbstractValidator<CreateTimeEntryCommand>
{
    public CreateTimeEntryCommandValidator()
    {
        RuleFor(command => command.HoursWorked)
            .InclusiveBetween(1, 24)
                .WithMessage("Hours worked must be in range <1, 24>");
    }
}
