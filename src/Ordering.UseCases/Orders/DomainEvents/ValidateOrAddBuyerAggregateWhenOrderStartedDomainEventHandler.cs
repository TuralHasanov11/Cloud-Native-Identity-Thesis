using Ordering.Contracts.Abstractions;

namespace Ordering.UseCases.Orders.DomainEvents;

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
            new GetCustomerByIdSpecification(new IdentityId(domainEvent.UserId)),
            cancellationToken);

        if (customer is null)
        {
            customer = Customer.Create(new IdentityId(domainEvent.UserId), domainEvent.UserName);
            await customerRepository.CreateAsync(customer, cancellationToken);
        }

        customer.VerifyOrAddPaymentMethod(
            cardTypeId,
            $"Payment Method on {DateTime.UtcNow}",
            domainEvent.CardNumber,
            domainEvent.CardSecurityNumber,
            domainEvent.CardHolderName,
            domainEvent.CardExpiration,
            domainEvent.Order.Id);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var integrationEvent = new OrderStatusChangedToSubmittedIntegrationEvent(
            domainEvent.Order.Id,
            domainEvent.Order.OrderStatus.ToString(),
            customer.Name,
            customer.IdentityId);

        await orderingIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
        //OrderingApiTrace.LogOrderBuyerAndPaymentValidatedOrUpdated(logger, customer.Id, domainEvent.Order.Id);
    }
}
