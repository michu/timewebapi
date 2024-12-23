namespace TimeWebApi.Features.Common.Exceptions;

public abstract class FeatureException : Exception
{
    public FeatureException(string message, IDictionary<string, string[]>? errors = null)
        : base(message)
    {
        Errors = errors?.AsReadOnly();
    }

    public IReadOnlyDictionary<string, string[]>? Errors { get; private set; }
}
