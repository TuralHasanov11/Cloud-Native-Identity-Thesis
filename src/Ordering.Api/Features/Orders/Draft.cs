namespace Ordering.Api.Features.Orders;

public static class Draft
{
    public static async Task<Results<Ok<OrderDraftDto>, ProblemHttpResult>> Handle(
        IOrderRepository orderRepository,
        DraftOrderRequest request,
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

        return TypedResults.Ok(order.ToOrderDraftDto());
    }
}
