namespace Catalog.Contracts.IntegrationEvents;

public record OrderStockConfirmedIntegrationEvent(Guid OrderId) : IntegrationEvent;
