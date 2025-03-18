namespace Ordering.Infrastructure.Repositories;

public class CustomerRepository(OrderingDbContext dbContext) : ICustomerRepository
{
    private readonly OrderingDbContext _dbContext = dbContext;

    //public Buyer Update(Buyer buyer)
    //{
    //    return _dbContext.Buyers
    //            .Update(buyer)
    //            .Entity;
    //}

    //public async Task<Buyer> FindAsync(string identity)
    //{
    //    var buyer = await _dbContext.Buyers
    //        .Include(b => b.PaymentMethods)
    //        .Where(b => b.IdentityGuid == identity)
    //        .SingleOrDefaultAsync();

    //    return buyer;
    //}

    //public async Task<Buyer> FindByIdAsync(int id)
    //{
    //    var buyer = await _dbContext.Buyers
    //        .Include(b => b.PaymentMethods)
    //        .Where(b => b.Id == id)
    //        .SingleOrDefaultAsync();

    //    return buyer;
    //}

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
        Func<Customer, TResponse> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class
    {
        return await _dbContext.Customers
            .GetQuery(specification)
            .Select(c => mapper(c))
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
        Func<Customer, TResponse> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class
    {
        return await _dbContext.Customers
            .GetQuery(specification)
            .Select(c => mapper(c))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task CreateAsync(
        Customer customer,
        CancellationToken cancellationToken = default)
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
}
