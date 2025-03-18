namespace Ordering.Contracts.IntegrationEvents.Events;

public sealed record OrderStatusChangedToSubmittedIntegrationEvent(
    Guid OrderId,
    int OrderStatus,
    Guid CustomerName,
    Guid CustomerIdentityId) : IntegrationEvent;
