namespace TimeWebApi.Behaviours;

using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Collections.Generic;

public sealed class ValidationPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResults = new List<ValidationResult>();

        foreach (var validator in _validators)
        {
            validationResults.Add(await validator.ValidateAsync(context, cancellationToken));
        }

        var errors = validationResults
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .GroupBy(x => x.PropertyName, x => x.ErrorMessage, (propertyName, errorMessages) => new
            {
                Key = propertyName,
                Values = errorMessages.Distinct().ToArray()
            })
            .ToDictionary(x => x.Key, x => x.Values);

        if (errors.Count > 0)
        {
            throw new Features.Common.Exceptions.ValidationException(errors);
        }

        var response = await next();

        return response;
    }
}
