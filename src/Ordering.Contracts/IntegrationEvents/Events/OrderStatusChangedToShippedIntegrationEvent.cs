namespace Ordering.Contracts.IntegrationEvents.Events;

public sealed record OrderStatusChangedToShippedIntegrationEvent(
    Guid OrderId,
    int OrderStatus,
    Guid CustomerName,
    Guid CustomerIdentityId) : IntegrationEvent;
