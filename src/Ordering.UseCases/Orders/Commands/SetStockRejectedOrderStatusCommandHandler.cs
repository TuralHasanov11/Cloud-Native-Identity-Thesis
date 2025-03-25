namespace Ordering.UseCases.Orders.Commands;

public class SetStockRejectedOrderStatusCommandHandler(
    IOrderRepository orderRepository)
    : ICommandHandler<SetStockRejectedOrderStatusCommand, bool>
{
    public async Task<Result<bool>> Handle(
        SetStockRejectedOrderStatusCommand request,
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.SingleOrDefaultAsync(
            new GetOrderByIdSpecification(new OrderId(request.OrderNumber)),
            cancellationToken);

        if (order == null)
        {
            return Result.NotFound();
        }

        order.Cancel(request.OrderStockItems);

        orderRepository.Update(order);

        await orderRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(true);
    }
}
