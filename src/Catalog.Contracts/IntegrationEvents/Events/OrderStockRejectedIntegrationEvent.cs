namespace Catalog.Contracts.IntegrationEvents.Events;

public record OrderStockRejectedIntegrationEvent(
    Guid OrderId,
    List<ConfirmedOrderStockItem> OrderStockItems)
    : IntegrationEvent;
