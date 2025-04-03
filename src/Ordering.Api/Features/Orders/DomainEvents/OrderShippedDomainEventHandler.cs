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
        //OrderingApiTrace.LogOrderStatusUpdated(_logger, domainEvent.Order.Id, OrderStatus.Shipped);

        var order = await orderRepository.SingleOrDefaultAsync(
            new GetOrderByIdSpecification(domainEvent.Order.Id),
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

        var integrationEvent = new OrderStatusChangedToShippedIntegrationEvent(
            order.Id,
            order.OrderStatus.ToString(),
            customer.Name,
            customer.IdentityId);

        await orderingIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
    }
}
