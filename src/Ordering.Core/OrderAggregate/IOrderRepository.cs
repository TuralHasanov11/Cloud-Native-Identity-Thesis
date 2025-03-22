using System.Linq.Expressions;

namespace Ordering.Core.OrderAggregate;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> ListAsync(
        Specification<Order> specification,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TResponse>> ListAsync<TResponse>(
        Specification<Order> specification,
        Expression<Func<Order, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class;

    Task<Order?> SingleOrDefaultAsync(
        Specification<Order> specification,
        CancellationToken cancellationToken = default);

    Task<TResponse?> SingleOrDefaultAsync<TResponse>(
        Specification<Order> specification,
        Expression<Func<Order, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class;

    Task CreateAsync(
        Order order,
        CancellationToken cancellationToken = default);

    void Update(Order order);

    void Delete(Order order);
}
