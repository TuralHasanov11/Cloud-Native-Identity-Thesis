using System.Linq.Expressions;

namespace Catalog.Infrastructure.Repositories;

public class ProductTypeRepository(CatalogDbContext dbContext) : IProductTypeRepository
{
    public async Task CreateAsync(ProductType productType, CancellationToken cancellationToken = default)
    {
        await dbContext.ProductTypes.AddAsync(productType, cancellationToken);
    }

    public void Delete(ProductType productType)
    {
        dbContext.ProductTypes.Remove(productType);
    }

    public Task<int> DeleteAsync(
        Specification<ProductType> specification,
        CancellationToken cancellationToken = default)
    {
        return dbContext.ProductTypes.GetQuery(specification).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductType>> ListAsync(
        Specification<ProductType> specification,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.ProductTypes.GetQuery(specification).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TResponse>> ListAsync<TResponse>(
        Specification<ProductType> specification,
        Expression<Func<ProductType, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class
    {
        return await dbContext.ProductTypes.GetQuery(specification).Select(mapper).ToListAsync(cancellationToken);
    }

    public Task<ProductType?> SingleOrDefaultAsync(
        Specification<ProductType> specification,
        CancellationToken cancellationToken = default)
    {
        return dbContext.ProductTypes.GetQuery(specification).SingleOrDefaultAsync(cancellationToken);
    }

    public Task<TResponse?> SingleOrDefaultAsync<TResponse>(
        Specification<ProductType> specification,
        Expression<Func<ProductType, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class
    {
        return dbContext.ProductTypes.GetQuery(specification).Select(mapper).SingleOrDefaultAsync(cancellationToken);
    }

    public void Update(ProductType productType)
    {
        dbContext.ProductTypes.Update(productType);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
