using Microsoft.EntityFrameworkCore;

namespace Ordering.Core.OrderAggregate.Specifications;

public class OrderSpecification : Specification<Order>
{
    public OrderSpecification(OrderId id)
        : base(o => o.Id == id)
    {
        AddInclude(order => order.Include(o => o.OrderItems));
    }

    public OrderSpecification(IdentityId userId)
       : base(o => o.Customer.IdentityId == userId)
    {
        AddInclude(order => order.Include(o => o.OrderItems));
    }
}
