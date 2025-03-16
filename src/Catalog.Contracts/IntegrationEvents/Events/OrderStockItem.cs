namespace Catalog.Contracts.IntegrationEvents.Events;

public record OrderStockItem(Guid ProductId, int Units);
