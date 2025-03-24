using System.Linq.Expressions;

namespace Catalog.Core.CatalogAggregate;

public interface IProductTypeRepository
{
    Task<IEnumerable<ProductType>> ListAsync(
        Specification<ProductType> specification,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TResponse>> ListAsync<TResponse>(
        Specification<ProductType> specification,
        Expression<Func<ProductType, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class;

    Task<ProductType?> SingleOrDefaultAsync(
        Specification<ProductType> specification,
        CancellationToken cancellationToken = default);

    Task<TResponse?> SingleOrDefaultAsync<TResponse>(
        Specification<ProductType> specification,
        Expression<Func<ProductType, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class;

    Task CreateAsync(ProductType productType, CancellationToken cancellationToken = default);

    void Update(ProductType productType);

    void Delete(ProductType productType);

    Task<int> DeleteAsync(Specification<ProductType> specification, CancellationToken cancellationToken = default);
}
