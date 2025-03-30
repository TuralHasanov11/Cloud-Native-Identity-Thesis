namespace Catalog.Contracts.IntegrationEvents;

public sealed record OrderStatusChangedToAwaitingValidationIntegrationEvent(
    Guid OrderId,
    IEnumerable<OrderStockItem> OrderStockItems)
    : IntegrationEvent;
