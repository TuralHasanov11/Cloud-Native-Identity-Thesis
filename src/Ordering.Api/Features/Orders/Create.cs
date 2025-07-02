using ServiceDefaults.Identity;

namespace Ordering.Api.Features.Orders;

public static class Create
{
    public static async Task<Results<Ok, ProblemHttpResult>> Handle(
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IOrderingIntegrationEventService orderingIntegrationEventService,
        CreateOrderRequest request,
        ILogger<CreateOrderRequest> logger,
        IIdentityService identityService,
        CancellationToken cancellationToken)
    {
        var userId = new IdentityId(identityService.GetUser()!.GetUserId()!);

        var customer = await customerRepository
            .SingleOrDefaultAsync(
                new CustomerSpecification(userId),
                cancellationToken: cancellationToken);

        if (customer is null)
        {
            await customerRepository.CreateAsync(
                Customer.Create(userId, request.UserName),
                cancellationToken);

            await orderRepository.SaveChangesAsync(cancellationToken);
        }

        var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent(userId);
        await orderingIntegrationEventService.AddAndSaveEventAsync(orderStartedIntegrationEvent);

        var order = new Order(
            userId,
            request.UserName,
            new Address(request.Street, request.City, request.State, request.Country, request.ZipCode),
            request.CardTypeId,
            customerId: customer?.Id);

        foreach (var item in request.Items.ToOrderItemsDto())
        {
            order.AddOrderItem(
                item.ProductId,
                item.ProductName,
                item.UnitPrice,
                item.Discount,
                item.PictureUrl,
                item.Units);
        }

        logger.LogCreatingOrder(order);

        await orderRepository.CreateAsync(order, cancellationToken);

        await orderRepository.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok();
    }
}

public static partial class CreateOrderRequestLogger
{
    [LoggerMessage(LogLevel.Information, "Creating Order - Order: {Order}", EventName = "CreatingOrder")]
    public static partial void LogCreatingOrder(this ILogger<CreateOrderRequest> logger, Order order);
}
