namespace Ordering.Contracts.IntegrationEvents;

public sealed record OrderStatusChangedToStockConfirmedIntegrationEvent(
    Guid OrderId,
    string OrderStatus,
    string CustomerName,
    Guid CustomerIdentityId) : IntegrationEvent;
