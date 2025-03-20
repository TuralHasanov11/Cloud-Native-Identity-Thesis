namespace Ordering.Contracts.IntegrationEvents;

public sealed record OrderStockRejectedIntegrationEvent(
    Guid OrderId,
    IReadOnlyCollection<ConfirmedOrderStockItem> OrderStockItems) : IntegrationEvent;

public sealed record ConfirmedOrderStockItem(Guid ProductId, bool HasStock);
