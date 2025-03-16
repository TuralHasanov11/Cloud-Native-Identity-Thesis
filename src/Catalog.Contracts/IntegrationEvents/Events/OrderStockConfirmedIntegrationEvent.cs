namespace Catalog.Contracts.IntegrationEvents.Events;

public record OrderStockConfirmedIntegrationEvent(Guid OrderId) : IntegrationEvent;
