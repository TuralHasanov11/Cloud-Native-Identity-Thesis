using System.Linq.Expressions;

namespace Ordering.Core.CustomerAggregate;

public interface ICardTypeRepository
{
    Task<IEnumerable<CardType>> ListAsync(
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TResponse>> ListAsync<TResponse>(
        Expression<Func<CardType, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class;

    Task<CardType?> SingleOrDefaultAsync(
        Specification<CardType> specification,
        CancellationToken cancellationToken = default);

    Task<TResponse?> SingleOrDefaultAsync<TResponse>(
        Specification<CardType> specification,
        Expression<Func<CardType, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class;

    Task CreateAsync(
        CardType order,
        CancellationToken cancellationToken = default);

    void Update(CardType order);

    void Delete(CardType order);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
