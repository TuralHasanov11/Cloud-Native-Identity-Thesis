namespace Catalog.Contracts.IntegrationEvents.Events;

public record OrderStatusChangedToPaidIntegrationEvent(
    int OrderId,
    IEnumerable<OrderStockItem> OrderStockItems)
    : IntegrationEvent;
