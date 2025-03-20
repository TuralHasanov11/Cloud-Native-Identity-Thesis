namespace Ordering.UseCases.Orders.Queries;

public sealed record GetOrderByIdQuery(Guid Id) : IQuery<OrderDto>;
