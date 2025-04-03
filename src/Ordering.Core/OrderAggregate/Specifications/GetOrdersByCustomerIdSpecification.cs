using Microsoft.EntityFrameworkCore;

namespace Ordering.Core.OrderAggregate.Specifications;

public class GetOrdersByCustomerIdSpecification : Specification<Order>
{
    public GetOrdersByCustomerIdSpecification(IdentityId userId)
        : base(o => o.Customer.IdentityId == userId)
    {
        AddInclude(order => order.Include(o => o.OrderItems));
    }
}
