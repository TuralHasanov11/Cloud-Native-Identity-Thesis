namespace Ordering.Contracts.IntegrationEvents;

public sealed record OrderStockConfirmedIntegrationEvent(Guid OrderId) : IntegrationEvent;
