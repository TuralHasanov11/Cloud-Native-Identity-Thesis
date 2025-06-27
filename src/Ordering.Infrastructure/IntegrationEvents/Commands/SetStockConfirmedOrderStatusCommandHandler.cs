namespace Ordering.Infrastructure.IntegrationEvents.Commands;

public sealed class SetStockConfirmedOrderStatusCommandHandler(
    IOrderRepository orderRepository)
        : ICommandHandler<SetStockConfirmedOrderStatusCommand, bool>
{

    public async Task<Result<bool>> Handle(SetStockConfirmedOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.SingleOrDefaultAsync(
            new OrderSpecification(new OrderId(request.OrderNumber)),
            cancellationToken);

        if (order == null)
        {
            return Result.NotFound();
        }

        order.ConfirmStock();

        orderRepository.Update(order);

        await orderRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(true);
    }
}
