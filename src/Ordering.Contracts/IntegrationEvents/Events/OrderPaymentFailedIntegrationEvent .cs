namespace Ordering.Contracts.IntegrationEvents.Events;

public sealed record OrderPaymentFailedIntegrationEvent(Guid OrderId) : IntegrationEvent;
