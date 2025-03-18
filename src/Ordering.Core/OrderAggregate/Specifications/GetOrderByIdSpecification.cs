using Microsoft.EntityFrameworkCore;

namespace Ordering.Core.OrderAggregate.Specifications;

public class GetOrderByIdSpecification : Specification<Order>
{
    public GetOrderByIdSpecification(OrderId id)
        : base(o => o.Id == id)
    {
        AddInclude(order => order.Include(o => o.OrderItems));
    }
}
