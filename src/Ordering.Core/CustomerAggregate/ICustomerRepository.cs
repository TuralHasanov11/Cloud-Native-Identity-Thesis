using System.Linq.Expressions;

namespace Ordering.Core.CustomerAggregate;
public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> ListAsync(
        Specification<Customer> specification,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TResponse>> ListAsync<TResponse>(
        Specification<Customer> specification,
        Expression<Func<Customer, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class;

    Task<Customer?> SingleOrDefaultAsync(
        Specification<Customer> specification,
        CancellationToken cancellationToken = default);

    Task<TResponse?> SingleOrDefaultAsync<TResponse>(
        Specification<Customer> specification,
        Expression<Func<Customer, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class;

    Task CreateAsync(
        Customer customer,
        CancellationToken cancellationToken = default);

    void Update(Customer customer);

    void Delete(Customer customer);

    void ForceDelete(Customer customer);

    Task<int> ForceDeleteAsync(
        Specification<Customer> specification,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
