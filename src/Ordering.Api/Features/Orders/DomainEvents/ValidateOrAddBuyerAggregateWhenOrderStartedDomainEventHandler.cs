namespace Ordering.Api.Features.Orders.DomainEvents;


public class ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler(
    ILogger<ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler> logger,
    ICustomerRepository customerRepository,
    IOrderingIntegrationEventService orderingIntegrationEventService,
    IUnitOfWork unitOfWork)
    : IDomainEventHandler<OrderStartedDomainEvent>
{
    public async Task Handle(OrderStartedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var cardTypeId = domainEvent.CardTypeId != 0 ? domainEvent.CardTypeId : 1; // TODO: set the default card type to 1 for now

        var customer = await customerRepository.SingleOrDefaultAsync(
            new CustomerSpecification(new IdentityId(domainEvent.UserId)),
            cancellationToken);

        if (customer is null)
        {
            customer = Customer.Create(new IdentityId(domainEvent.UserId), domainEvent.UserName);
            await customerRepository.CreateAsync(customer, cancellationToken);
        }

        customer.VerifyOrAddPaymentMethod(
            cardTypeId,
            $"Payment Method on {DateTime.UtcNow}",
            domainEvent.Order.Id);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var integrationEvent = new OrderStatusChangedToSubmittedIntegrationEvent(
            domainEvent.Order.Id,
            domainEvent.Order.OrderStatus.ToString(),
            customer.Name,
            customer.IdentityId);

        await orderingIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
        logger.LogOrderBuyerAndPaymentValidatedOrUpdated(customer.Id, domainEvent.Order.Id);
    }
}

public static partial class ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandlerLogger
{
    [LoggerMessage(
        LogLevel.Information,
        EventId = 1002,
        Message = "Order {OrderId} buyer and payment method validated or updated for customer {CustomerId}.")]
    public static partial void LogOrderBuyerAndPaymentValidatedOrUpdated(
        this ILogger<ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler> logger,
        Guid customerId,
        Guid orderId);
}
