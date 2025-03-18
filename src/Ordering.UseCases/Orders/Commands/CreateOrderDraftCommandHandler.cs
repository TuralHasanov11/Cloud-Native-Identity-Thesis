namespace Ordering.UseCases.Orders.Commands;

public sealed class CreateOrderDraftCommandHandler : ICommandHandler<CreateOrderDraftCommand, OrderDraftDto>
{
    public Task<Result<OrderDraftDto>> Handle(CreateOrderDraftCommand request, CancellationToken cancellationToken)
    {
        var order = Order.NewDraft();
        var orderItems = request.Items.Select(i => i.ToOrderItemDto());
        foreach (var item in orderItems)
        {
            order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.PictureUrl, item.Units);
        }

        return Task.FromResult(Result.Success(order.ToOrderDraftDto()));
    }
}
