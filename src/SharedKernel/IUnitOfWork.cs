using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SharedKernel;

public interface IUnitOfWork
{
    EntityEntry<TEntity> GetEntry<TEntity>(TEntity entity)
        where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
