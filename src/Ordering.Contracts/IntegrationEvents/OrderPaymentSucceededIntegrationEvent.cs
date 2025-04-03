namespace Ordering.Contracts.IntegrationEvents;

public sealed record OrderPaymentSucceededIntegrationEvent(Guid OrderId) : IntegrationEvent;
