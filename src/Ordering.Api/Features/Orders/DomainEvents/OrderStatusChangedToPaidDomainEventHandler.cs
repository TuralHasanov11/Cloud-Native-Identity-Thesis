namespace Ordering.Api.Features.Orders.DomainEvents;


public sealed class OrderStatusChangedToPaidDomainEventHandler(
    IOrderRepository orderRepository,
    ICustomerRepository customerRepository,
    IOrderingIntegrationEventService orderingIntegrationEventService)
    : IDomainEventHandler<OrderStatusChangedToPaidDomainEvent>
{
    public async Task Handle(OrderStatusChangedToPaidDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        //OrderingApiTrace.LogOrderStatusUpdated(_logger, domainEvent.OrderId, OrderStatus.Paid);

        var order = await orderRepository.SingleOrDefaultAsync(
            new GetOrderByIdSpecification(new OrderId(domainEvent.OrderId)),
            cancellationToken);

        if (order == null)
        {
            return;
        }

        var customer = await customerRepository.SingleOrDefaultAsync(
            new GetCustomerByIdSpecification(new CustomerId(order.CustomerId!.Value)), // TODO: handle null
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
