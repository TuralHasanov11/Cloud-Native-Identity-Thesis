using System.Linq.Expressions;

namespace Catalog.Infrastructure.Repositories;

public class ProductRepository(CatalogDbContext dbContext) : IProductRepository
{
    public async Task CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        await dbContext.Products.AddAsync(product, cancellationToken);
    }

    public void Delete(Product product)
    {
        dbContext.Products.Remove(product);
    }

    public Task<int> DeleteAsync(Specification<Product> specification, CancellationToken cancellationToken = default)
    {
        return dbContext.Products.GetQuery(specification).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<(IEnumerable<Product>, long)> ListAsync(
        Specification<Product> specification,
        ProductId pageCursor,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var (dataTask, countTask) = dbContext.Products.GetQuery(specification)
            .OrderBy(p => p.Id)
            .Paginate(pageCursor, field: p => p.Id, pageSize, cancellationToken);

        return (await dataTask, await countTask);
    }

    public async Task<IEnumerable<Product>> ListAsync(
        Specification<Product> specification,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Products.GetQuery(specification).ToListAsync(cancellationToken);
    }

    //public async Task<(IEnumerable<TResponse>, long)> ListAsync<TResponse>(
    //    Specification<Product> specification,
    //    Expression<Func<Product, TResponse>> mapper,
    //    ProductId pageCursor,
    //    int pageSize = 10,
    //    CancellationToken cancellationToken = default)
    //    where TResponse : class
    //{
    //    var (dataTask, countTask) = dbContext.Products.GetQuery(specification)
    //        .OrderBy(p => p.Id)
    //        .Select(mapper)
    //        .Paginate(pageCursor, field: p => p.Id, pageSize, cancellationToken);

    //    return (await dataTask, await countTask);
    //}

    public async Task<IEnumerable<TResponse>> ListAsync<TResponse>(
        Specification<Product> specification,
        Expression<Func<Product, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class
    {
        return await dbContext.Products.GetQuery(specification).Select(mapper).ToListAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Product?> SingleOrDefaultAsync(
        Specification<Product> specification,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Products.GetQuery(specification).SingleOrDefaultAsync(cancellationToken);
    }

    public Task<TResponse?> SingleOrDefaultAsync<TResponse>(
        Specification<Product> specification,
        Expression<Func<Product, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class
    {
        return dbContext.Products.GetQuery(specification).Select(mapper).SingleOrDefaultAsync(cancellationToken);
    }

    public void Update(Product product)
    {
        dbContext.Products.Update(product);
    }
}
