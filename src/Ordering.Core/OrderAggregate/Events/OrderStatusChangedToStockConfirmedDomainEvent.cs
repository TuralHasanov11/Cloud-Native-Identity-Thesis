namespace Ordering.Core.OrderAggregate.Events;

public sealed record OrderStatusChangedToStockConfirmedDomainEvent(
    OrderId OrderId,
    DateTime OccurredOnUtc)
    : DomainEventBase(OccurredOnUtc);
