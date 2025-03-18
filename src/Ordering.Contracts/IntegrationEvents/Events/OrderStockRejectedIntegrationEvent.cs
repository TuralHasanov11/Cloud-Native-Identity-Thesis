namespace Ordering.Contracts.IntegrationEvents.Events;

public sealed record OrderStockRejectedIntegrationEvent(
    Guid OrderId,
    IReadOnlyCollection<ConfirmedOrderStockItem> OrderStockItems) : IntegrationEvent;

public sealed record ConfirmedOrderStockItem(Guid ProductId, bool HasStock);
