namespace Ordering.Contracts.IntegrationEvents.Events;

public sealed record OrderStatusChangedToStockConfirmedIntegrationEvent(
    Guid OrderId,
    int OrderStatus,
    Guid CustomerName,
    Guid CustomerIdentityId) : IntegrationEvent;
