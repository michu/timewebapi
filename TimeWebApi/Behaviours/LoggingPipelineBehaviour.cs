namespace TimeWebApi.Behaviours;

using MediatR;
using TimeWebApi.Resources;

public sealed class LoggingPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull
{
    private readonly ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> _logger;

    public LoggingPipelineBehaviour(ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Starting request: {RequestName} at {DateTime}", requestName, DateTime.UtcNow.ToString(StaticData.DateTimeFormats.UtcIso));

        try
        {
            var response = await next();

            _logger.LogInformation("Completed request: {RequestName} at {DateTime}", requestName, DateTime.UtcNow.ToString(StaticData.DateTimeFormats.UtcIso));

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Request {RequestName} failed at {DateTime}", requestName, DateTime.UtcNow.ToString(StaticData.DateTimeFormats.UtcIso));

            throw;
        }
    }
}
