using Ordering.Core.OrderAggregate.Specifications;

namespace Ordering.Api.Features.Orders;

public static class Ship
{
    public static async Task<Results<Ok, ProblemHttpResult>> Handle(
        IOrderRepository orderRepository,
        ShipOrderRequest request,
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.SingleOrDefaultAsync(
            new GetOrderByIdSpecification(new OrderId(request.OrderNumber)),
            cancellationToken);

        if (order == null)
        {
            return TypedResults.Problem();
        }

        order.Ship();

        orderRepository.Update(order);

        await orderRepository.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok();
    }
}
