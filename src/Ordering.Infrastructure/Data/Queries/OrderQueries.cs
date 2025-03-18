namespace Ordering.Infrastructure.Data.Queries;

public class OrderQueries(OrderingDbContext context)
    : IOrderQueries
{
    public async Task<OrderDto> GetOrderAsync(Guid id)
    {
        var orderId = new OrderId(id);

        var order = await context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order is null)
        {
            throw new KeyNotFoundException();
        }

        return order.ToOrderDto();
    }

    public async Task<IEnumerable<OrderSummary>> GetOrdersFromUserAsync(Guid userId)
    {
        return await context.Orders
            .Where(o => o.Customer.IdentityId == userId)
            .Select(o => new OrderSummary(
                o.Id,
                o.OrderDate,
                o.OrderStatus.ToString(),
                o.OrderItems.Sum(oi => oi.UnitPrice * oi.Units)))
            .ToListAsync();
    }

    public async Task<IEnumerable<CardType>> GetCardTypesAsync()
    {
        return await context.CardTypes.ToListAsync();
    }
}
