namespace Ordering.Contracts.IntegrationEvents.Events;

public sealed record OrderStatusChangedToCanceledIntegrationEvent(
    Guid OrderId,
    int OrderStatus,
    string CustomerName,
    Guid CustomerIdentityId) : IntegrationEvent;
