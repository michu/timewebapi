﻿namespace TimeWebApi.Features.Common.Messaging;

using MediatR;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}
