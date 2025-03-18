namespace Ordering.Core.Events;

public sealed record OrderCanceledDomainEvent(Order Order, DateTime OccurredOnUtc)
    : DomainEventBase(OccurredOnUtc);
