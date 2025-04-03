namespace Ordering.Contracts.IntegrationEvents;

public sealed record OrderStatusChangedToPaidIntegrationEvent(
    Guid OrderId,
    string OrderStatus,
    string CustomerName,
    string CustomerIdentityId,
    IEnumerable<OrderStockItem> OrderStockItems) : IntegrationEvent;
