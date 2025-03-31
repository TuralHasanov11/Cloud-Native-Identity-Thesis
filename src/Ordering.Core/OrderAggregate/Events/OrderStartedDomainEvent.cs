namespace Ordering.Core.OrderAggregate.Events;

public sealed record OrderStartedDomainEvent(
    Order Order,
    string UserId,
    string UserName,
    int CardTypeId,
    string CardNumber,
    string CardSecurityNumber,
    string CardHolderName,
    DateTime CardExpiration,
    DateTime OccurredOnUtc) : DomainEventBase(OccurredOnUtc);
