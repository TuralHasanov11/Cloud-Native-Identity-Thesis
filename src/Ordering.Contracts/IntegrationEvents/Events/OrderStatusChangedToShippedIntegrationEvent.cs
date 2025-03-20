namespace Ordering.Contracts.IntegrationEvents.Events;

public sealed record OrderStatusChangedToShippedIntegrationEvent(
    Guid OrderId,
    string OrderStatus,
    string CustomerName,
    Guid CustomerIdentityId) : IntegrationEvent;
