namespace Ordering.UseCases.Orders.Commands;

public sealed class CreateOrderDraftCommandHandler(
    IOrderRepository orderRepository)
    : ICommandHandler<CreateOrderDraftCommand, OrderDraftDto>
{
    public async Task<Result<OrderDraftDto>> Handle(
        CreateOrderDraftCommand request,
        CancellationToken cancellationToken)
    {
        var order = Order.NewDraft();
        var orderItems = request.Items.Select(i => i.ToOrderItemDto());
        foreach (var item in orderItems)
        {
            order.AddOrderItem(
                item.ProductId,
                item.ProductName,
                item.UnitPrice,
                item.Discount,
                item.PictureUrl,
                item.Units);
        }

        await orderRepository.CreateAsync(order, cancellationToken);
        await orderRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(order.ToOrderDraftDto());
    }
}
