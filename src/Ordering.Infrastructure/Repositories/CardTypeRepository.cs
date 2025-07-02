using System.Linq.Expressions;

namespace Ordering.Infrastructure.Repositories;

public class CardTypeRepository(OrderingDbContext dbContext) : ICardTypeRepository
{
    public async Task CreateAsync(CardType order, CancellationToken cancellationToken = default)
    {
        await dbContext.CardTypes.AddAsync(order, cancellationToken);
    }

    public void Delete(CardType order)
    {
        dbContext.CardTypes.Remove(order);
    }

    public async Task<IEnumerable<CardType>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.CardTypes.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TResponse>> ListAsync<TResponse>(
        Expression<Func<CardType, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class
    {
        return await dbContext.CardTypes.Select(mapper).AsNoTracking().ToListAsync(cancellationToken);
    }

    public Task<CardType?> SingleOrDefaultAsync(
        Specification<CardType> specification,
        CancellationToken cancellationToken = default)
    {
        return dbContext.CardTypes.GetQuery(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<TResponse?> SingleOrDefaultAsync<TResponse>(
        Specification<CardType> specification,
        Expression<Func<CardType, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class
    {
        return dbContext.CardTypes
            .GetQuery(specification)
            .Select(mapper)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public void Update(CardType order)
    {
        dbContext.CardTypes.Update(order);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
