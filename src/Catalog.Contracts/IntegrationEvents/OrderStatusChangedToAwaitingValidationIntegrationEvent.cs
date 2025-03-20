namespace Catalog.Contracts.IntegrationEvents;

public record OrderStatusChangedToAwaitingValidationIntegrationEvent(
    Guid OrderId,
    IEnumerable<OrderStockItem> OrderStockItems)
    : IntegrationEvent;
