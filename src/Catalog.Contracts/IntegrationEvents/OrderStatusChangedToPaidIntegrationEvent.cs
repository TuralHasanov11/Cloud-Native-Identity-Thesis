namespace Catalog.Contracts.IntegrationEvents;

public sealed record OrderStatusChangedToPaidIntegrationEvent(
    Guid OrderId,
    IEnumerable<OrderStockItem> OrderStockItems)
    : IntegrationEvent;
