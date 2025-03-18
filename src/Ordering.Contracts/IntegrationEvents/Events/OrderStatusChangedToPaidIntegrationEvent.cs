namespace Ordering.Contracts.IntegrationEvents.Events;

public sealed record OrderStatusChangedToPaidIntegrationEvent(
    Guid OrderId,
    int OrderStatus,
    Guid CustomerName,
    Guid CustomerIdentityId,
    IEnumerable<OrderStockItem> OrderStockItems) : IntegrationEvent;

