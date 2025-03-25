namespace Ordering.UseCases.Orders.Commands;

public class CancelOrderCommandHandler(IOrderRepository orderRepository)
    : ICommandHandler<CancelOrderCommand, bool>
{
    public async Task<Result<bool>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.SingleOrDefaultAsync(
            new GetOrderByIdSpecification(new OrderId(request.OrderNumber)),
            cancellationToken);

        if (order is null)
        {
            return Result<bool>.NotFound();
        }

        order.Cancel();

        orderRepository.Update(order);

        await orderRepository.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
