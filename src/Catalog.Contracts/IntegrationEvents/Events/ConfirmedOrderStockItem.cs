namespace Catalog.Contracts.IntegrationEvents.Events;

public record ConfirmedOrderStockItem(Guid ProductId, bool HasStock);
