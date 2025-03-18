using Ordering.Core.CustomerAggregate;
using Ordering.Core.CustomerAggregate.Specifications;
using Ordering.Core.Events;

namespace Ordering.UseCases.Orders.DomainEvents;

public sealed class OrderCancelledDomainEventHandler : IDomainEventHandler<OrderCanceledDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger _logger;
    private readonly IOrderingIntegrationEventService _orderingIntegrationEventService;

    public OrderCancelledDomainEventHandler(
        IOrderRepository orderRepository,
        ILogger<OrderCancelledDomainEventHandler> logger,
        ICustomerRepository customerRepository,
        IOrderingIntegrationEventService orderingIntegrationEventService)
    {
        _orderRepository = orderRepository;
        _logger = logger;
        _customerRepository = customerRepository;
        _orderingIntegrationEventService = orderingIntegrationEventService;
    }

    public async Task Handle(OrderCanceledDomainEvent notification, CancellationToken cancellationToken)
    {
        OrderingApiTrace.LogOrderStatusUpdated(_logger, notification.Order.Id, OrderStatus.Cancelled);

        var order = await _orderRepository.SingleOrDefaultAsync(
            new GetOrderByIdSpecification(new OrderId(notification.Order.Id)),
            cancellationToken);

        if (order == null)
        {
            return;
        }

        var customer = await _customerRepository.SingleOrDefaultAsync(
            new GetCustomerByIdSpecification(order.CustomerId),
            cancellationToken);

        if (customer == null)
        {
            return;
        }

        await _orderingIntegrationEventService.AddAndSaveEventAsync(
            new OrderStatusChangedToCanceledIntegrationEvent(
                order.Id,
                (int)order.OrderStatus,
                customer.Name,
                customer.IdentityId));
    }
}
