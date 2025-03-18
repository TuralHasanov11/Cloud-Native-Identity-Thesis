namespace Ordering.Infrastructure.Data.Queries;

public interface IOrderQueries
{
    Task<OrderDto> GetOrderAsync(Guid id);

    Task<IEnumerable<OrderSummary>> GetOrdersFromUserAsync(Guid userId);

    Task<IEnumerable<CardType>> GetCardTypesAsync();
}
