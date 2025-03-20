using Ordering.Contracts.Abstractions;

namespace Ordering.UseCases.Orders.Commands;

public class CreateOrderCommandHandler(
    IOrderingIntegrationEventService orderingIntegrationEventService,
    IOrderRepository orderRepository,
    ILogger<CreateOrderCommandHandler> logger,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IOrderingIntegrationEventService _orderingIntegrationEventService = orderingIntegrationEventService;
    private readonly ILogger<CreateOrderCommandHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<bool>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent(request.UserId);
        await _orderingIntegrationEventService.AddAndSaveEventAsync(orderStartedIntegrationEvent);


        var order = new Order(
            request.UserId,
            request.UserName,
            new Address(request.Street, request.City, request.State, request.Country, request.ZipCode),
            request.CardTypeId,
            request.CardNumber,
            request.CardSecurityNumber,
            request.CardHolderName,
            request.CardExpiration);

        foreach (var item in request.OrderItems)
        {
            order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.PictureUrl, item.Units);
        }

        _logger.LogCreatingOrder(order);

        await _orderRepository.CreateAsync(order, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}

public static partial class CreateOrderCommandHandlerLogger
{
    [LoggerMessage(LogLevel.Information, "Creating Order - Order: {Order}", EventName = "CreatingOrder")]
    public static partial void LogCreatingOrder(this ILogger<CreateOrderCommandHandler> logger, Order order);
}


// Use for Idempotency in Command process
//public class CreateOrderIdentifiedCommandHandler : IdentifiedCommandHandler<CreateOrderCommand, bool>
//{
//    public CreateOrderIdentifiedCommandHandler(
//        IMediator mediator,
//        IRequestManager requestManager,
//        ILogger<IdentifiedCommandHandler<CreateOrderCommand, bool>> logger)
//        : base(mediator, requestManager, logger)
//    {
//    }

//    protected override bool CreateResultForDuplicateRequest()
//    {
//        return true; // Ignore duplicate requests for creating order.
//    }
//}
