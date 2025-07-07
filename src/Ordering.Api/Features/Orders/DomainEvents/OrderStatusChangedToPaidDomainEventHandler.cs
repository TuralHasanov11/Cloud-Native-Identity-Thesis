namespace Ordering.Api.Features.Orders.DomainEvents;


public sealed class OrderStatusChangedToPaidDomainEventHandler(
    IOrderRepository orderRepository,
    ICustomerRepository customerRepository,
    ILogger<OrderStatusChangedToPaidDomainEventHandler> logger,
    IOrderingIntegrationEventService orderingIntegrationEventService)
    : IDomainEventHandler<OrderStatusChangedToPaidDomainEvent>
{
    public async Task Handle(OrderStatusChangedToPaidDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogOrderStatusUpdated(domainEvent.OrderId, OrderStatus.Paid);

        var order = await orderRepository.SingleOrDefaultAsync(
            new OrderSpecification(new OrderId(domainEvent.OrderId)),
            cancellationToken);

        if (order == null)
        {
            return;
        }

        ArgumentNullException.ThrowIfNull(order.CustomerId);

        var customer = await customerRepository.SingleOrDefaultAsync(
            new CustomerSpecification(new CustomerId(order.CustomerId)),
            cancellationToken);

        if (customer == null)
        {
            return;
        }

        var orderStockList = domainEvent.OrderItems
            .Select(orderItem => new OrderStockItem(orderItem.ProductId, orderItem.Units));

        var integrationEvent = new OrderStatusChangedToPaidIntegrationEvent(
            domainEvent.OrderId,
            order.OrderStatus.ToString(),
            customer.Name,
            customer.IdentityId,
            orderStockList);

        await orderingIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
    }
}

public static partial class OrderStatusChangedToPaidDomainEventHandlerLogger
{
    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Information,
        Message = "Order status updated to {OrderStatus} for order {OrderId}")]
    public static partial void LogOrderStatusUpdated(this ILogger<OrderStatusChangedToPaidDomainEventHandler> logger, Guid orderId, OrderStatus orderStatus);
}
