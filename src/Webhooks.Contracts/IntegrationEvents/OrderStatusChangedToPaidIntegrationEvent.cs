namespace Catalog.Contracts.IntegrationEvents;

public record OrderStatusChangedToPaidIntegrationEvent(
    int OrderId,
    IEnumerable<OrderStockItem> OrderStockItems)
    : IntegrationEvent;
