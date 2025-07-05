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
            new OrderSpecification(new OrderId(domainEvent.OrderId)),
            cancellationToken);

        if (order == null)
        {
            return;
        }

        order.VerifyPayment(domainEvent.Customer.Id, domainEvent.PaymentMethod.Id);
        logger.LogOrderPaymentMethodUpdated(domainEvent.OrderId, nameof(domainEvent.PaymentMethod.Alias), domainEvent.PaymentMethod.Id);
    }
}

public static partial class UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandlerLogger
{
    [LoggerMessage(
        LogLevel.Information,
        EventId = 1001,
        Message = "Order {OrderId} payment method updated to {PaymentType} with ID {PaymentId}.")]
    public static partial void LogOrderPaymentMethodUpdated(
        this ILogger<UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler> logger,
        Guid orderId,
        string paymentType,
        Guid paymentId);
}
