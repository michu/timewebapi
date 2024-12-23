namespace TimeWebApi.Behaviours;

using MediatR;
using Npgsql;
using System.Data;
using TimeWebApi.Features.Common.Messaging;
using TimeWebApi.Resources;

public sealed class TransactionPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, ITransactionCommand<TResponse>
{
    private readonly NpgsqlConnection _connection;
    private readonly ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> _logger;

    public TransactionPipelineBehaviour(NpgsqlConnection connection, ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> logger)
    {
        _connection = connection;
        _logger = logger;
    }

#pragma warning disable CS8603 // Possible null reference return.
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var response = default(TResponse);

        using var transaction = await _connection.BeginTransactionAsync(IsolationLevel.ReadUncommitted, cancellationToken);

        _logger.LogInformation("Begin transaction for request: {RequestName} at {DateTime}", requestName, DateTime.UtcNow.ToString(StaticData.DateTimeFormats.UtcIso));

        try
        {
            response = await next();

            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation("Commited transaction for request: {RequestName} at {DateTime}", requestName, DateTime.UtcNow.ToString(StaticData.DateTimeFormats.UtcIso));
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);

            _logger.LogInformation("Rolled back transaction for request: {RequestName} at {DateTime}", requestName, DateTime.UtcNow.ToString(StaticData.DateTimeFormats.UtcIso));

            throw;
        }

        return response;
    }
#pragma warning restore CS8603 // Possible null reference return.
}
