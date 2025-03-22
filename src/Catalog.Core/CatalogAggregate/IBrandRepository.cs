using System.Linq.Expressions;

namespace Catalog.Core.CatalogAggregate;

public interface IBrandRepository
{
    Task<IEnumerable<Brand>> ListAsync(
        Specification<Brand> specification,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TResponse>> ListAsync<TResponse>(
        Specification<Brand> specification,
        Expression<Func<Brand, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class;

    Task<Brand?> SingleOrDefaultAsync(
        Specification<Brand> specification,
        CancellationToken cancellationToken = default);

    Task<TResponse?> SingleOrDefaultAsync<TResponse>(
        Specification<Brand> specification,
        Expression<Func<Brand, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class;

    Task CreateAsync(
        Brand brand,
        CancellationToken cancellationToken = default);

    void Update(Brand brand);

    void Delete(Brand brand);

    Task<int> DeleteAsync(Specification<Brand> specification, CancellationToken cancellationToken = default);
}
