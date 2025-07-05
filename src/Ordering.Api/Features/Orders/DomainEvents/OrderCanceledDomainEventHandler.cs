namespace Ordering.Api.Features.Orders.DomainEvents;

public sealed class OrderCanceledDomainEventHandler(
    IOrderRepository orderRepository,
    ILogger<OrderCanceledDomainEventHandler> logger,
    ICustomerRepository customerRepository,
    IOrderingIntegrationEventService orderingIntegrationEventService) : IDomainEventHandler<OrderCanceledDomainEvent>
{
    public async Task Handle(OrderCanceledDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogOrderStatusUpdated(notification.Order.Id, OrderStatus.Cancelled);

        var order = await orderRepository.SingleOrDefaultAsync(
            new OrderSpecification(new OrderId(notification.Order.Id)),
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

        await orderingIntegrationEventService.AddAndSaveEventAsync(
            new OrderStatusChangedToCanceledIntegrationEvent(
                order.Id,
                (int)order.OrderStatus,
                customer.Name,
                customer.IdentityId));
    }
}

public static partial class OrderCanceledDomainEventHandlerLogger
{
    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Information,
        Message = "Order status updated to {OrderStatus} for order {OrderId}")]
    public static partial void LogOrderStatusUpdated(this ILogger<OrderCanceledDomainEventHandler> logger, Guid orderId, OrderStatus orderStatus);
}

