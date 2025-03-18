using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Ordering.Infrastructure.Data;

public class UnitOfWork(OrderingDbContext dbContext) : IUnitOfWork
{
    private readonly OrderingDbContext _dbContext = dbContext;

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
