namespace Ordering.Contracts.IntegrationEvents.Events;

public sealed record OrderStatusChangedToAwaitingValidationIntegrationEvent(
    Guid OrderId,
    int OrderStatus,
    string CustomerName,
    Guid CustomerIdentityId,
    IEnumerable<OrderStockItem> OrderStockItems) : IntegrationEvent;

public sealed record OrderStockItem(Guid ProductId, int Units);
