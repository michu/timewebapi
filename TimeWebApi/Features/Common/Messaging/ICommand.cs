namespace TimeWebApi.Features.Common.Messaging;

using MediatR;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
