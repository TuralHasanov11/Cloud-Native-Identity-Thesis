using MediatR;

namespace SharedKernel;

public abstract record DomainEventBase(DateTime OccurredOnUtc) : INotification;

public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : DomainEventBase;
