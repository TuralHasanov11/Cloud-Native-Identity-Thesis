namespace Ordering.Contracts.IntegrationEvents;

public sealed record OrderStatusChangedToAwaitingValidationIntegrationEvent(
    Guid OrderId,
    string OrderStatus,
    string CustomerName,
    string CustomerIdentityId,
    IEnumerable<OrderStockItem> OrderStockItems) : IntegrationEvent;

public sealed record OrderStockItem(Guid ProductId, int Units);
