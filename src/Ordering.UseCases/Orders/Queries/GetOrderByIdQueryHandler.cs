namespace Ordering.UseCases.Orders.Queries;

public sealed class GetOrderByIdQueryHandler(
    IOrderRepository orderRepository)
    : IQueryHandler<GetOrderByIdQuery, OrderDto>
{
    public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var orderId = new OrderId(request.Id);

        var order = await orderRepository.SingleOrDefaultAsync(
            new GetOrderByIdSpecification(orderId),
            cancellationToken);

        if (order is null)
        {
            return Result<OrderDto>.NotFound();
        }

        return order.ToOrderDto();
    }
}
