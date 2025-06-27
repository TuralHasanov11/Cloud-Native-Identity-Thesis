namespace Ordering.Api.Features.Orders.DomainEvents;


public sealed class OrderStatusChangedToAwaitingValidationDomainEventHandler(
    IOrderRepository orderRepository,
    ILogger<OrderStatusChangedToAwaitingValidationDomainEventHandler> logger,
    ICustomerRepository customerRepository,
    IOrderingIntegrationEventService orderingIntegrationEventService)
        : IDomainEventHandler<OrderStatusChangedToAwaitingValidationDomainEvent>
{
    public async Task Handle(
        OrderStatusChangedToAwaitingValidationDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        //OrderingApiTrace.LogOrderStatusUpdated(_logger, domainEvent.OrderId, OrderStatus.AwaitingValidation);

        var order = await orderRepository.SingleOrDefaultAsync(
            new OrderSpecification(new OrderId(domainEvent.OrderId)),
            cancellationToken);

        if (order == null)
        {
            return;
        }

        var customer = await customerRepository.SingleOrDefaultAsync(
            new CustomerSpecification(new CustomerId(order.CustomerId!.Value)), // TODO: handle null
            cancellationToken);

        if (customer == null)
        {
            return;
        }

        var orderStockList = domainEvent.OrderItems
            .Select(orderItem => new OrderStockItem(orderItem.ProductId, orderItem.Units));

        var integrationEvent = new OrderStatusChangedToAwaitingValidationIntegrationEvent(
            order.Id,
            order.OrderStatus.ToString(),
            customer.Name,
            customer.IdentityId,
            orderStockList);

        await orderingIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
    }
}
