namespace Webhooks.Contracts.IntegrationEvents;

public record OrderStatusChangedToPaidIntegrationEvent(
    Guid OrderId,
    IEnumerable<OrderStockItem> OrderStockItems)
    : IntegrationEvent;
