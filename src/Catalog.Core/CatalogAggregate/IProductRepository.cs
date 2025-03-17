namespace Catalog.Core.CatalogAggregate;

public interface IProductRepository
{
    Task<(IEnumerable<Product>, long)> ListAsync(
        Specification<Product> specification,
        ProductId pageCursor,
        int pageSize = 10,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Product>> ListAsync(
        Specification<Product> specification,
        CancellationToken cancellationToken = default);

    Task<(IEnumerable<TResponse>, long)> ListAsync<TResponse>(
        Specification<Product> specification,
        Func<Product, TResponse> mapper,
        ProductId pageCursor,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
        where TResponse : class;

    Task<IEnumerable<TResponse>> ListAsync<TResponse>(
        Specification<Product> specification,
        Func<Product, TResponse> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class;

    Task<Product?> SingleOrDefaultAsync(
        Specification<Product> specification,
        CancellationToken cancellationToken = default);

    Task<TResponse?> SingleOrDefaultAsync<TResponse>(
        Specification<Product> specification,
        Func<Product, TResponse> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class;

    Task CreateAsync(
        Product course,
        CancellationToken cancellationToken = default);

    void Update(Product course);

    void Delete(Product course);

    void ForceDelete(Product course);

    Task<int> ForceDeleteAsync(
        Specification<Product> specification,
        CancellationToken cancellationToken = default);
}
