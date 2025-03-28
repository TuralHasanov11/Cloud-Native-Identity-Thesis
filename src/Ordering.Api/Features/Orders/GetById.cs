namespace Ordering.Api.Features.Orders;

public static class GetById
{
    public static async Task<Results<Ok<OrderDto>, NotFound>> Handle(
        IOrderRepository orderRepository,
        Guid id,
        CancellationToken cancellationToken)
    {
        var orderId = new OrderId(id);

        var order = await orderRepository.SingleOrDefaultAsync(
            new GetOrderByIdSpecification(orderId),
            cancellationToken);

        if (order is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(order.ToOrderDto());
    }
}
