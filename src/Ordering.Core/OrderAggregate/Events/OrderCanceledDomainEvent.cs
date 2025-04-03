namespace Ordering.Core.OrderAggregate.Events;

public sealed record OrderCanceledDomainEvent(Order Order, DateTime OccurredOnUtc)
    : DomainEventBase(OccurredOnUtc);
