namespace Ordering.Core.CustomerAggregate.Events;

public sealed record CustomerAndPaymentMethodVerifiedDomainEvent(
    Customer Customer,
    PaymentMethod Payment,
    OrderId OrderId,
    DateTime OccurredOnUtc)
    : DomainEventBase(OccurredOnUtc);
