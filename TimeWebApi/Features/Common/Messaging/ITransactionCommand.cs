namespace TimeWebApi.Features.Common.Messaging;

public interface ITransactionCommand<out TResponse> : ICommand<TResponse>
{
}
