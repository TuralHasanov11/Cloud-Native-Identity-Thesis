namespace Ordering.Core.Events;

public sealed record OrderShippedDomainEvent(Order Order, DateTime OccurredOnUtc)
    : DomainEventBase(OccurredOnUtc);
