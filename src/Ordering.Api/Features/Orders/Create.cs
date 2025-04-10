namespace Ordering.Api.Features.Orders;

public static class Create
{
    public static async Task<Results<Ok, ProblemHttpResult>> Handle(
        IOrderRepository orderRepository,
        IOrderingIntegrationEventService orderingIntegrationEventService,
        CreateOrderRequest request,
        ILogger<CreateOrderRequest> logger,
        CancellationToken cancellationToken)
    {
        var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent(request.UserId);
        await orderingIntegrationEventService.AddAndSaveEventAsync(orderStartedIntegrationEvent);

        var order = new Order(
            new IdentityId(request.UserId),
            request.UserName,
            new Address(request.Street, request.City, request.State, request.Country, request.ZipCode),
            request.CardTypeId,
            customerId: new CustomerId(request.Customer));

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
