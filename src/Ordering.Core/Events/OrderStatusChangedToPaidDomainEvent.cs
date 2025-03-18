namespace Ordering.Core.Events;

public sealed record OrderStatusChangedToPaidDomainEvent(
    OrderId OrderId,
    IEnumerable<OrderItem> OrderItems,
    DateTime OccurredOnUtc)
    : DomainEventBase(OccurredOnUtc);
