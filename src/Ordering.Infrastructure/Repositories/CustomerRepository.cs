using System.Linq.Expressions;

namespace Ordering.Infrastructure.Repositories;

public class CustomerRepository(OrderingDbContext dbContext) : ICustomerRepository
{
    private readonly OrderingDbContext _dbContext = dbContext;

    public async Task<IEnumerable<Customer>> ListAsync(
        Specification<Customer> specification,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Customers
            .GetQuery(specification)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TResponse>> ListAsync<TResponse>(
        Specification<Customer> specification,
        Expression<Func<Customer, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class
    {
        return await _dbContext.Customers
            .GetQuery(specification)
            .Select(mapper)
            .ToListAsync(cancellationToken);
    }

    public async Task<Customer?> SingleOrDefaultAsync(
        Specification<Customer> specification,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Customers
            .GetQuery(specification)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TResponse?> SingleOrDefaultAsync<TResponse>(
        Specification<Customer> specification,
        Expression<Func<Customer, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class
    {
        return await _dbContext.Customers
            .GetQuery(specification)
            .Select(mapper)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task CreateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await _dbContext.Customers.AddAsync(customer, cancellationToken);
    }

    public void Update(Customer customer)
    {
        _dbContext.Customers.Update(customer);
    }

    public void Delete(Customer customer)
    {
        _dbContext.Customers.Remove(customer);
    }

    public void ForceDelete(Customer customer)
    {
        _dbContext.Customers.Remove(customer);
    }

    public async Task<int> ForceDeleteAsync(
        Specification<Customer> specification,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Customers
            .GetQuery(specification)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
