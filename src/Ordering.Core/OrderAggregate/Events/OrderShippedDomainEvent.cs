namespace Ordering.Core.OrderAggregate.Events;

public sealed record OrderShippedDomainEvent(Order Order, DateTime OccurredOnUtc)
    : DomainEventBase(OccurredOnUtc);
