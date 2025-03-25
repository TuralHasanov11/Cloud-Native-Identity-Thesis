using Ordering.Contracts.Abstractions;

namespace Ordering.UseCases.Orders.Commands;

public class CreateOrderCommandHandler(
    IOrderingIntegrationEventService orderingIntegrationEventService,
    IOrderRepository orderRepository,
    ILogger<CreateOrderCommandHandler> logger)
    : ICommandHandler<CreateOrderCommand, bool>
{
    public async Task<Result<bool>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent(request.UserId);
        await orderingIntegrationEventService.AddAndSaveEventAsync(orderStartedIntegrationEvent);


        var order = new Order(
            new IdentityId(request.UserId),
            request.UserName,
            new Address(request.Street, request.City, request.State, request.Country, request.ZipCode),
            request.CardTypeId,
            request.CardNumber,
            request.CardSecurityNumber,
            request.CardHolderName,
            request.CardExpiration);

        foreach (var item in request.OrderItems)
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

        return Result<bool>.Success(true);
    }
}

public static partial class CreateOrderCommandHandlerLogger
{
    [LoggerMessage(LogLevel.Information, "Creating Order - Order: {Order}", EventName = "CreatingOrder")]
    public static partial void LogCreatingOrder(this ILogger<CreateOrderCommandHandler> logger, Order order);
}
