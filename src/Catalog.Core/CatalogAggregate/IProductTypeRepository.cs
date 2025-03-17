namespace Catalog.Core.CatalogAggregate;

public interface IProductTypeRepository
{
    Task<IEnumerable<ProductType>> ListAsync(
        Specification<ProductType> specification,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TResponse>> ListAsync<TResponse>(
        Specification<ProductType> specification,
        Func<ProductType, TResponse> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class;

    Task<ProductType?> SingleOrDefaultAsync(
        Specification<ProductType> specification,
        CancellationToken cancellationToken = default);

    Task<TResponse?> SingleOrDefaultAsync<TResponse>(
        Specification<ProductType> specification,
        Func<ProductType, TResponse> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class;

    Task CreateAsync(
        ProductType course,
        CancellationToken cancellationToken = default);

    void Update(ProductType course);

    void Delete(ProductType course);

    void ForceDelete(ProductType course);

    Task<int> ForceDeleteAsync(
        Specification<ProductType> specification,
        CancellationToken cancellationToken = default);
}
