namespace OrderProcessor.Events;

public sealed record GracePeriodConfirmedIntegrationEvent(Guid OrderId) : IntegrationEvent;
