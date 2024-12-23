namespace TimeWebApi.Features.TimeEntries.Commands.UpdateTimeEntry;

using FluentValidation;

public sealed class UpdateTimeEntryCommandValidator : AbstractValidator<UpdateTimeEntryCommand>
{
    public UpdateTimeEntryCommandValidator()
    {
        RuleFor(command => command.HoursWorked)
            .InclusiveBetween(1, 24)
                .WithMessage("Hours worked must be in range <1, 24>");
    }
}
