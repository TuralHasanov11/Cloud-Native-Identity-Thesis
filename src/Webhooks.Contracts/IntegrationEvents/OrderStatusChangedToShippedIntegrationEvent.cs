namespace Webhooks.Contracts.IntegrationEvents;

public sealed record OrderStatusChangedToShippedIntegrationEvent(int OrderId, string OrderStatus, string BuyerName)
    : IntegrationEvent;
