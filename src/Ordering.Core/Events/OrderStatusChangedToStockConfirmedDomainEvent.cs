namespace Ordering.Core.Events;

public sealed record OrderStatusChangedToStockConfirmedDomainEvent(
    OrderId OrderId,
    DateTime OccurredOnUtc)
    : DomainEventBase(OccurredOnUtc);
