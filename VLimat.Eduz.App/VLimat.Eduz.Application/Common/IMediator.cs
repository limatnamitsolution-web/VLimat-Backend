using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLimat.Eduz.Application.Common
{
    public interface IMediator
    {
        Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
        Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
        Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent;
    }
    // Application/Common/ICommand.cs
    public interface ICommand<TResult> { }

    // Application/Common/IQuery.cs
    public interface IQuery<TResult> { }

    // Application/Common/IEvent.cs
    public interface IEvent { }

    // Application/Common/ICommandHandler.cs
    public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
    }

    // Application/Common/IQueryHandler.cs
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> Handle(TQuery query, CancellationToken cancellationToken);
    }

    // Application/Common/IEventHandler.cs
    public interface IEventHandler<TEvent> where TEvent : IEvent
    {
        Task Handle(TEvent @event, CancellationToken cancellationToken);
    }
}
