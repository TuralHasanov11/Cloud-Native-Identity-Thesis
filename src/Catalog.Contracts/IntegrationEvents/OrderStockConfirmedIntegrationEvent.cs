namespace Catalog.Contracts.IntegrationEvents;

public sealed record OrderStockConfirmedIntegrationEvent(Guid OrderId) : IntegrationEvent;
