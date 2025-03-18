namespace Ordering.Core.Events;

public sealed record CustomerAndPaymentMethodVerifiedDomainEvent(
    Customer Buyer,
    PaymentMethod Payment,
    OrderId OrderId,
    DateTime OccurredOnUtc)
    : DomainEventBase(OccurredOnUtc);
