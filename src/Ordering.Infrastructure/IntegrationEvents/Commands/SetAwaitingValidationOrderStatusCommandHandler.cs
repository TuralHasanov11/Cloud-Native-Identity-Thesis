namespace Ordering.Infrastructure.IntegrationEvents.Commands;


public class SetAwaitingValidationOrderStatusCommandHandler(
    IOrderRepository orderRepository)
        : ICommandHandler<SetAwaitingValidationOrderStatusCommand, bool>
{
    public async Task<Result<bool>> Handle(
        SetAwaitingValidationOrderStatusCommand request,
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.SingleOrDefaultAsync(
            new OrderSpecification(new OrderId(request.OrderNumber)),
            cancellationToken);

        if (order == null)
        {
            return Result.NotFound();
        }

        order.SetAwaitingValidationStatus();

        orderRepository.Update(order);

        await orderRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(true);
    }
}
