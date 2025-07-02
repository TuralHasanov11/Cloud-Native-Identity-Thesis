namespace Ordering.Api.Features.Orders;

public static class Cancel
{
    public static async Task<Results<Ok, ProblemHttpResult>> Handle(
        IOrderRepository orderRepository,
        CancelOrderRequest request,
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.SingleOrDefaultAsync(
            new OrderSpecification(new OrderId(request.OrderNumber)),
            cancellationToken);

        if (order is not null)
        {
            order.Cancel();

            orderRepository.Update(order);

            await orderRepository.SaveChangesAsync(cancellationToken);

            return TypedResults.Ok();
        }

        return TypedResults.Problem(
            new ProblemDetails()
            {
                Title = "Order cancellation failed"
            });
    }
}
