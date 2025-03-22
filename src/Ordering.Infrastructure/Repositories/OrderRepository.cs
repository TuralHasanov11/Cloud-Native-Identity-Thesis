using System.Linq.Expressions;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository(OrderingDbContext dbContext) : IOrderRepository
{
    private readonly OrderingDbContext _dbContext = dbContext;

    public async Task CreateAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _dbContext.Orders.AddAsync(order, cancellationToken);
    }

    public void Delete(Order order)
    {
        _dbContext.Orders.Remove(order);
    }

    public async Task<IEnumerable<Order>> ListAsync(
        Specification<Order> specification,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .GetQuery(specification)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TResponse>> ListAsync<TResponse>(
        Specification<Order> specification,
        Expression<Func<Order, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class
    {
        return await _dbContext.Orders
            .GetQuery(specification)
            .Select(mapper)
            .ToListAsync(cancellationToken);
    }

    public async Task<Order?> SingleOrDefaultAsync(
        Specification<Order> specification,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .GetQuery(specification)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TResponse?> SingleOrDefaultAsync<TResponse>(
        Specification<Order> specification,
        Expression<Func<Order, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class
    {
        return await _dbContext.Orders
            .GetQuery(specification)
            .Select(mapper)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public void Update(Order order)
    {
        _dbContext.Orders.Update(order);
    }
}
