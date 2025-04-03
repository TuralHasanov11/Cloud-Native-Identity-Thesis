namespace Webhooks.Contracts.IntegrationEvents;

public record OrderStockItem(Guid ProductId, int Units);
