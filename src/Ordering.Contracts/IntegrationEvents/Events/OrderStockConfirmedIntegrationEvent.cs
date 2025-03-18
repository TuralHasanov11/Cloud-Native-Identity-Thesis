namespace Ordering.Contracts.IntegrationEvents.Events;

public sealed record OrderStockConfirmedIntegrationEvent(Guid OrderId) : IntegrationEvent;
