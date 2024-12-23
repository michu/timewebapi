namespace TimeWebApi.Features.Common.Exceptions;

public sealed class ValidationException : FeatureException
{
    public ValidationException(IDictionary<string, string[]> errors)
        : base("One or more validation errors has occurred", errors)
    {
    }
}
