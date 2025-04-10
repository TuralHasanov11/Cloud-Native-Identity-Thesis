namespace Ordering.Core.OrderAggregate.Events;

public sealed record OrderStartedDomainEvent(
    Order Order,
    string UserId,
    string UserName,
    int CardTypeId,
    DateTime OccurredOnUtc) : DomainEventBase(OccurredOnUtc);
