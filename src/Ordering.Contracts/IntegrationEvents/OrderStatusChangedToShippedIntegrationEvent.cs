namespace Ordering.Contracts.IntegrationEvents;

public sealed record OrderStatusChangedToShippedIntegrationEvent(
    Guid OrderId,
    string OrderStatus,
    string CustomerName,
    string CustomerIdentityId) : IntegrationEvent;
