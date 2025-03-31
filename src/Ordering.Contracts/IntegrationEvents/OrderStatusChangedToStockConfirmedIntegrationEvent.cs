namespace Ordering.Contracts.IntegrationEvents;

public sealed record OrderStatusChangedToStockConfirmedIntegrationEvent(
    Guid OrderId,
    string OrderStatus,
    string CustomerName,
    string CustomerIdentityId) : IntegrationEvent;
