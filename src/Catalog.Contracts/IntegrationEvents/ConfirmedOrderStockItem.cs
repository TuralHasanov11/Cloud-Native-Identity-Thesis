namespace Catalog.Contracts.IntegrationEvents;

public record ConfirmedOrderStockItem(Guid ProductId, bool HasStock) : IntegrationEvent;
