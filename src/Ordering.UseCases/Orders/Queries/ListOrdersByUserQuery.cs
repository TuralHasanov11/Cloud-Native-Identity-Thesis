namespace Ordering.UseCases.Orders.Queries;

public sealed record ListOrdersByUserQuery(Guid UserId) : IQuery<IEnumerable<OrderSummary>>;
