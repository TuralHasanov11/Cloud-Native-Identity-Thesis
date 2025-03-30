namespace Catalog.Contracts.IntegrationEvents;

public sealed record ConfirmedOrderStockItemIntegrationEvent(Guid ProductId, bool HasStock)
    : IntegrationEvent;
