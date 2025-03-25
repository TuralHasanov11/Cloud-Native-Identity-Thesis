namespace Ordering.UseCases.Orders.Queries;

public sealed class ListOrdersByUserQueryHandler(
    IOrderRepository orderRepository)
    : IQueryHandler<ListOrdersByUserQuery, IEnumerable<OrderSummary>>
{
    public async Task<Result<IEnumerable<OrderSummary>>> Handle(ListOrdersByUserQuery request, CancellationToken cancellationToken)
    {
        var orders = await orderRepository.ListAsync(
            new GetOrdersByCustomerIdSpecification(new IdentityId(request.UserId)),
            cancellationToken);

        return Result.Success(orders.Select(o => o.ToOrderSummary()));
    }
}
