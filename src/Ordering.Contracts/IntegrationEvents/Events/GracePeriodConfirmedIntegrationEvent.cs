namespace Ordering.Contracts.IntegrationEvents.Events;

public sealed record GracePeriodConfirmedIntegrationEvent(Guid OrderId) : IntegrationEvent;
