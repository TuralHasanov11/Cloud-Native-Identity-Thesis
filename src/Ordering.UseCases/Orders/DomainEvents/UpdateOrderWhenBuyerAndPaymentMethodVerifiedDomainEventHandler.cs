namespace Ordering.UseCases.Orders.DomainEvents;

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

        order.VerifyPayment(domainEvent.Customer.Id, domainEvent.Payment.Id);
        //OrderingApiTrace.LogOrderPaymentMethodUpdated(logger, domainEvent.OrderId, nameof(domainEvent.Payment), domainEvent.Payment.Id);
    }
}
