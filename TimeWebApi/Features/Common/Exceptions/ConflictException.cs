namespace TimeWebApi.Features.Common.Exceptions;

public sealed class ConflictException : FeatureException
{
    public ConflictException(string message): base(message)
    {
    }
}
