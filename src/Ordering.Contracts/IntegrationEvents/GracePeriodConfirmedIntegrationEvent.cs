namespace Ordering.Contracts.IntegrationEvents;

public sealed record GracePeriodConfirmedIntegrationEvent(Guid OrderId) : IntegrationEvent;
