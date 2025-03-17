using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Catalog.Infrastructure.Data;

public class UnitOfWork(CatalogDbContext dbContext) : IUnitOfWork
{
    private readonly CatalogDbContext _dbContext = dbContext;

    public EntityEntry<TEntity> GetEntry<TEntity>(TEntity entity)
        where TEntity : class
    {
        return _dbContext.Entry(entity);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
