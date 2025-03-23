namespace Webhooks.Contracts.IntegrationEvents;

public record OrderStatusChangedToShippedIntegrationEvent(int OrderId, string OrderStatus, string BuyerName) : IntegrationEvent;
