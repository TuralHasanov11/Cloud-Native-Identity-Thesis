namespace Ordering.Contracts.IntegrationEvents;

public sealed record OrderStatusChangedToCanceledIntegrationEvent(
    Guid OrderId,
    int OrderStatus,
    string CustomerName,
    string CustomerIdentityId) : IntegrationEvent;
