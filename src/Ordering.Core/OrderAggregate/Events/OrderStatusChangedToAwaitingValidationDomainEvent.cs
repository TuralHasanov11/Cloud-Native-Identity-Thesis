namespace Ordering.Core.OrderAggregate.Events;

public sealed record OrderStatusChangedToAwaitingValidationDomainEvent(
    OrderId OrderId,
    IEnumerable<OrderItem> OrderItems,
    DateTime OccurredOnUtc)
    : DomainEventBase(OccurredOnUtc);
