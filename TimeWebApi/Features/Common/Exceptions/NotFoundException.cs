namespace TimeWebApi.Features.Common.Exceptions;

public sealed class NotFoundException : FeatureException
{
    public NotFoundException(string message) : base(message)
    {
    }
}
