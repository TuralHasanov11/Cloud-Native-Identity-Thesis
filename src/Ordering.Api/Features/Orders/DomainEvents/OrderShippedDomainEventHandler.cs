namespace Ordering.Api.Features.Orders.DomainEvents;


public sealed class OrderShippedDomainEventHandler(
    IOrderRepository orderRepository,
    ILogger<OrderShippedDomainEventHandler> logger,
    ICustomerRepository customerRepository,
    IOrderingIntegrationEventService orderingIntegrationEventService)
    : IDomainEventHandler<OrderShippedDomainEvent>
{
    public async Task Handle(OrderShippedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogOrderStatusUpdated(domainEvent.Order.Id, OrderStatus.Shipped);

        var order = await orderRepository.SingleOrDefaultAsync(
            new OrderSpecification(domainEvent.Order.Id),
            cancellationToken);

        if (order == null)
        {
            return;
        }

        var customer = await customerRepository.SingleOrDefaultAsync(
            new CustomerSpecification(order.CustomerId!), // TODO: Check if CustomerId is nullable
            cancellationToken);

        if (customer == null)
        {
            return;
        }

        var integrationEvent = new OrderStatusChangedToShippedIntegrationEvent(
            order.Id,
            order.OrderStatus.ToString(),
            customer.Name,
            customer.IdentityId);

        await orderingIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
    }
}

public static partial class OrderShippedDomainEventHandlerLogger
{
    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Information,
        Message = "Order status updated to {OrderStatus} for order {OrderId}")]
    public static partial void LogOrderStatusUpdated(this ILogger<OrderShippedDomainEventHandler> logger, Guid orderId, OrderStatus orderStatus);
}
