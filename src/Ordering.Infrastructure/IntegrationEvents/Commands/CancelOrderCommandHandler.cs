namespace Ordering.Infrastructure.IntegrationEvents.Commands;

public sealed class CancelOrderCommandHandler(IOrderRepository orderRepository)
    : ICommandHandler<CancelOrderCommand, bool>
{
    public async Task<Result<bool>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.SingleOrDefaultAsync(
            new OrderSpecification(new OrderId(request.OrderNumber)),
            cancellationToken);

        if (order == null)
        {
            return Result.NotFound();
        }

        order.Cancel();
        orderRepository.Update(order);

        await orderRepository.SaveChangesAsync(cancellationToken);
        return Result.Success(true);
    }
}
