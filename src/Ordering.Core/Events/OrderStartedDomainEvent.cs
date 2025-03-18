namespace Ordering.Core.Events;

public sealed record OrderStartedDomainEvent(
    Order Order,
    Guid UserId,
    string UserName,
    int CardTypeId,
    string CardNumber,
    string CardSecurityNumber,
    string CardHolderName,
    DateTime CardExpiration,
    DateTime OccurredOnUtc) : DomainEventBase(OccurredOnUtc);
