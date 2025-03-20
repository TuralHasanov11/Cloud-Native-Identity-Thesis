namespace Ordering.Contracts.IntegrationEvents;

public sealed record OrderPaymentFailedIntegrationEvent(Guid OrderId) : IntegrationEvent;
