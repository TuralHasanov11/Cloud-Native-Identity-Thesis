namespace Ordering.UseCases.Orders.Commands;

public class ShipOrderCommandHandler(IOrderRepository orderRepository)
    : ICommandHandler<ShipOrderCommand, bool>
{
    public async Task<Result<bool>> Handle(ShipOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.SingleOrDefaultAsync(
            new GetOrderByIdSpecification(new OrderId(request.OrderNumber)),
            cancellationToken);

        if (order == null)
        {
            return Result.NotFound();
        }

        order.Ship();

        orderRepository.Update(order);

        await orderRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(true);
    }
}
