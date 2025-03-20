namespace Catalog.Contracts.IntegrationEvents;

public record OrderStockItem(Guid ProductId, int Units);
