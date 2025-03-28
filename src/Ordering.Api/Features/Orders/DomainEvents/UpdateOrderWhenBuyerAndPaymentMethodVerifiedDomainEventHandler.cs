using Ordering.Core.CustomerAggregate.Events;

namespace Ordering.Api.Features.Orders.DomainEvents;

public class UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler(
    IOrderRepository orderRepository,
    ILogger<UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler> logger)
    : IDomainEventHandler<CustomerAndPaymentMethodVerifiedDomainEvent>
{
    public async Task Handle(CustomerAndPaymentMethodVerifiedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var order = await orderRepository.SingleOrDefaultAsync(
            new GetOrderByIdSpecification(new OrderId(domainEvent.OrderId)),
            cancellationToken);

        if (order == null)
        {
            return;
        }

        order.VerifyPayment(domainEvent.Customer.Id, domainEvent.PaymentMethod.Id);
        //OrderingApiTrace.LogOrderPaymentMethodUpdated(logger, domainEvent.OrderId, nameof(domainEvent.Payment), domainEvent.Payment.Id);
    }
}
