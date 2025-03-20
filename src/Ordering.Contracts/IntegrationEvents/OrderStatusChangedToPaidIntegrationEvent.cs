namespace Ordering.Contracts.IntegrationEvents;

public sealed record OrderStatusChangedToPaidIntegrationEvent(
    Guid OrderId,
    string OrderStatus,
    string CustomerName,
    Guid CustomerIdentityId,
    IEnumerable<OrderStockItem> OrderStockItems) : IntegrationEvent;
