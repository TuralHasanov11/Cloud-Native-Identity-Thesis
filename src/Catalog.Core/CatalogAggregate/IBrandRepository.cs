namespace Catalog.Core.CatalogAggregate;

public interface IBrandRepository
{
    Task<IEnumerable<Brand>> ListAsync(
        Specification<Brand> specification,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TResponse>> ListAsync<TResponse>(
        Specification<Brand> specification,
        Func<Brand, TResponse> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class;

    Task<Brand?> SingleOrDefaultAsync(
        Specification<Brand> specification,
        CancellationToken cancellationToken = default);

    Task<TResponse?> SingleOrDefaultAsync<TResponse>(
        Specification<Brand> specification,
        Func<Brand, TResponse> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class;

    Task CreateAsync(
        Brand course,
        CancellationToken cancellationToken = default);

    void Update(Brand course);

    void Delete(Brand course);

    void ForceDelete(Brand course);

    Task<int> ForceDeleteAsync(
        Specification<Brand> specification,
        CancellationToken cancellationToken = default);
}
