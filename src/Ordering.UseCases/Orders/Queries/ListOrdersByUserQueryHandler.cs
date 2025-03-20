namespace Ordering.UseCases.Orders.Queries;

public sealed class ListOrdersByUserQueryHandler(
    IOrderRepository orderRepository)
    : IQueryHandler<ListOrdersByUserQuery, IEnumerable<OrderSummary>>
{
    public async Task<Result<IEnumerable<OrderSummary>>> Handle(ListOrdersByUserQuery request, CancellationToken cancellationToken)
    {
        var userId = request.Id;

        var orders = await orderRepository.ListAsync(
            new GetOrdersByCustomerIdSpecification(userId),
            cancellationToken);

        return Result.Success(orders.Select(o => o.ToOrderSummary()));
    }
}
