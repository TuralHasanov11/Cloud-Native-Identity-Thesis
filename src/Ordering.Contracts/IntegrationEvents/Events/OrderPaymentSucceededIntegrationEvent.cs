namespace Ordering.Contracts.IntegrationEvents.Events;

public sealed record OrderPaymentSucceededIntegrationEvent(Guid OrderId) : IntegrationEvent;
