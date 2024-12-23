namespace TimeWebApi.Features.Common.Messaging;

using MediatR;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
