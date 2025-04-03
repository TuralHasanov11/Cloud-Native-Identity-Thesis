namespace Ordering.Api.Features.Orders.DomainEvents;

public sealed class OrderCanceledDomainEventHandler(
    IOrderRepository orderRepository,
    ILogger<OrderCanceledDomainEventHandler> logger,
    ICustomerRepository customerRepository,
    IOrderingIntegrationEventService orderingIntegrationEventService) : IDomainEventHandler<OrderCanceledDomainEvent>
{
    public async Task Handle(OrderCanceledDomainEvent notification, CancellationToken cancellationToken)
    {
        //OrderingApiTrace.LogOrderStatusUpdated(_logger, notification.Order.Id, OrderStatus.Cancelled);

        var order = await orderRepository.SingleOrDefaultAsync(
            new GetOrderByIdSpecification(new OrderId(notification.Order.Id)),
            cancellationToken);

        if (order == null)
        {
            return;
        }

        var customer = await customerRepository.SingleOrDefaultAsync(
            new GetCustomerByIdSpecification(order.CustomerId!), // TODO: Check if CustomerId is nullable
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
