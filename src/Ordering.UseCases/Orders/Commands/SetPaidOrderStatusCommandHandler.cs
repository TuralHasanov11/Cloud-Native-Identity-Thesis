namespace Ordering.UseCases.Orders.Commands;

public class SetPaidOrderStatusCommandHandler(
    IOrderRepository orderRepository)
    : ICommandHandler<SetPaidOrderStatusCommand, bool>
{
    public async Task<Result<bool>> Handle(
        SetPaidOrderStatusCommand request,
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.SingleOrDefaultAsync(
            new GetOrderByIdSpecification(new OrderId(request.OrderNumber)),
            cancellationToken);

        if (order == null)
        {
            return Result.NotFound();
        }

        order.Pay();

        orderRepository.Update(order);

        await orderRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(true);
    }
}
