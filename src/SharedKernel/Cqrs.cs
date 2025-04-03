﻿using Ardalis.Result;
using MediatR;

namespace SharedKernel;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;

public interface IQueryHandler<in TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;

public interface ICommandHandler<in TCommand>
: IRequestHandler<TCommand, Result>
where TCommand : ICommand;

public interface ICommandHandler<in TCommand, TResponse>
: IRequestHandler<TCommand, Result<TResponse>>
where TCommand : ICommand<TResponse>;
