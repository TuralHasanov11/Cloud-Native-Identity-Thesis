namespace Ordering.Api.Features.Orders.DomainEvents;


public sealed class OrderStatusChangedToStockConfirmedDomainEventHandler(
    IOrderRepository orderRepository,
    ICustomerRepository customerRepository,
    ILogger<OrderStatusChangedToStockConfirmedDomainEventHandler> logger,
    IOrderingIntegrationEventService orderingIntegrationEventService)
    : IDomainEventHandler<OrderStatusChangedToStockConfirmedDomainEvent>
{
    public async Task Handle(OrderStatusChangedToStockConfirmedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogOrderStatusUpdated(domainEvent.OrderId, OrderStatus.StockConfirmed);

        var order = await orderRepository.SingleOrDefaultAsync(
            new OrderSpecification(new OrderId(domainEvent.OrderId)),
            cancellationToken);

        if (order == null)
        {
            return;
        }

        ArgumentNullException.ThrowIfNull(order.CustomerId);

        var customer = await customerRepository.SingleOrDefaultAsync(
            new CustomerSpecification(new CustomerId(order.CustomerId!.Value)),
            cancellationToken);

        if (customer == null)
        {
            return;
        }

        var integrationEvent = new OrderStatusChangedToStockConfirmedIntegrationEvent(
            order.Id,
            order.OrderStatus.ToString(),
            customer.Name,
            customer.IdentityId);

        await orderingIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
    }
}

public static partial class OrderStatusChangedToStockConfirmedDomainEventHandlerLogger
{
    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Information,
        Message = "Order status updated to {OrderStatus} for order {OrderId}")]
    public static partial void LogOrderStatusUpdated(this ILogger<OrderStatusChangedToStockConfirmedDomainEventHandler> logger, Guid orderId, OrderStatus orderStatus);
}
