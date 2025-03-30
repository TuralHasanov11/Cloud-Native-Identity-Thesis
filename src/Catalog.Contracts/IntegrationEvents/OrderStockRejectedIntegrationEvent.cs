namespace Catalog.Contracts.IntegrationEvents;

public sealed record OrderStockRejectedIntegrationEvent(
    Guid OrderId,
    List<ConfirmedOrderStockItemIntegrationEvent> OrderStockItems)
    : IntegrationEvent;
