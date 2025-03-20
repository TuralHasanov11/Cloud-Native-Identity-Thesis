namespace Ordering.Contracts.IntegrationEvents;

public sealed record OrderStatusChangedToSubmittedIntegrationEvent(
    Guid OrderId,
    string OrderStatus,
    string CustomerName,
    Guid CustomerIdentityId) : IntegrationEvent;
