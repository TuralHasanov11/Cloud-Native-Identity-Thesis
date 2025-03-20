using Ordering.Contracts.Abstractions;

namespace Ordering.UseCases.Orders.DomainEvents;

public sealed class OrderStatusChangedToStockConfirmedDomainEventHandler(
    IOrderRepository orderRepository,
    ICustomerRepository customerRepository,
    //ILogger<OrderStatusChangedToStockConfirmedDomainEventHandler> logger,
    IOrderingIntegrationEventService orderingIntegrationEventService)
    : IDomainEventHandler<OrderStatusChangedToStockConfirmedDomainEvent>
{
    public async Task Handle(OrderStatusChangedToStockConfirmedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        //OrderingApiTrace.LogOrderStatusUpdated(_logger, domainEvent.OrderId, OrderStatus.StockConfirmed);

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

        var integrationEvent = new OrderStatusChangedToStockConfirmedIntegrationEvent(
            order.Id,
            order.OrderStatus.ToString(),
            customer.Name,
            customer.IdentityId);

        await orderingIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
    }
}
