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

    public async Task<int> DeleteAsync(
        Specification<Brand> specification,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Brands
            .GetQuery(specification)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<IEnumerable<Brand>> ListAsync(
        Specification<Brand> specification,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Brands
            .GetQuery(specification)
            .AsNoTracking()
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
            .AsNoTracking()
            .Select(mapper)
            .ToListAsync(cancellationToken);
    }

    public async Task<Brand?> SingleOrDefaultAsync(
        Specification<Brand> specification,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Brands
            .GetQuery(specification)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<TResponse?> SingleOrDefaultAsync<TResponse>(
        Specification<Brand> specification,
        Expression<Func<Brand, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class
    {
        return await dbContext.Brands
            .GetQuery(specification)
            .Select(mapper)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public void Update(Brand brand)
    {
        dbContext.Brands.Update(brand);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
