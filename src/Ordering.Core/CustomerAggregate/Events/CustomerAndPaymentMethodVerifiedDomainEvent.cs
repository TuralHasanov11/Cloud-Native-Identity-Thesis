namespace Ordering.Core.CustomerAggregate.Events;

public sealed record CustomerAndPaymentMethodVerifiedDomainEvent(
    Customer Customer,
    PaymentMethod PaymentMethod,
    OrderId OrderId,
    DateTime OccurredOnUtc)
    : DomainEventBase(OccurredOnUtc);
