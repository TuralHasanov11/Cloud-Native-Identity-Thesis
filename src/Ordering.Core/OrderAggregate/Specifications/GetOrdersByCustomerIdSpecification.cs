using Microsoft.EntityFrameworkCore;

namespace Ordering.Core.OrderAggregate.Specifications;

public class GetOrdersByCustomerIdSpecification : Specification<Order>
{
    public GetOrdersByCustomerIdSpecification(Guid customerId)
        : base(o => o.Id == customerId)
    {
        AddInclude(order => order.Include(o => o.OrderItems));
    }
}
