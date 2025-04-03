namespace Webhooks.Contracts.IntegrationEvents;

public sealed record OrderStatusChangedToPaidIntegrationEvent(
    Guid OrderId,
    IEnumerable<OrderStockItem> OrderStockItems)
    : IntegrationEvent;
