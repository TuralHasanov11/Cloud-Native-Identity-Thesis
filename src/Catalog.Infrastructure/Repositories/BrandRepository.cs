using System.Linq.Expressions;

namespace Catalog.Infrastructure.Repositories;

public class BrandRepository(CatalogDbContext dbContext) : IBrandRepository
{
    public async Task CreateAsync(Brand brand, CancellationToken cancellationToken = default)
    {
        await dbContext.Brands.AddAsync(brand, cancellationToken);
    }

    public void Delete(Brand brand)
    {
        dbContext.Brands.Remove(brand);
    }

    public Task<int> DeleteAsync(
        Specification<Brand> specification,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Brands
            .GetQuery(specification)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<IEnumerable<Brand>> ListAsync(
        Specification<Brand> specification,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Brands
            .GetQuery(specification)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TResponse>> ListAsync<TResponse>(
        Specification<Brand> specification,
        Expression<Func<Brand, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class
    {
        return await dbContext.Brands
            .GetQuery(specification)
            .Select(mapper)
            .ToListAsync(cancellationToken);
    }

    public Task<Brand?> SingleOrDefaultAsync(
        Specification<Brand> specification,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Brands
            .GetQuery(specification)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Task<TResponse?> SingleOrDefaultAsync<TResponse>(
        Specification<Brand> specification,
        Expression<Func<Brand, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class
    {
        return dbContext.Brands
            .GetQuery(specification)
            .Select(mapper)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public void Update(Brand brand)
    {
        dbContext.Brands.Update(brand);
    }
}
