﻿namespace TimeWebApi.Features.Common.Messaging;

using MediatR;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
}
