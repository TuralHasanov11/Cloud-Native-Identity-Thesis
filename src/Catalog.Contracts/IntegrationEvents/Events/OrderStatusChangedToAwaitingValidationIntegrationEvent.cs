namespace Catalog.Contracts.IntegrationEvents.Events;

public record OrderStatusChangedToAwaitingValidationIntegrationEvent(
    Guid OrderId,
    IEnumerable<OrderStockItem> OrderStockItems)
    : IntegrationEvent;
