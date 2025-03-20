namespace Ordering.UseCases.Orders.Queries;

public sealed record ListOrdersByUserQuery(Guid Id) : IQuery<IEnumerable<OrderSummary>>;
