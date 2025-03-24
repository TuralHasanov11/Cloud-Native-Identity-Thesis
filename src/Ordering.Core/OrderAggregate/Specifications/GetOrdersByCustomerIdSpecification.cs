using Microsoft.EntityFrameworkCore;

namespace Ordering.Core.OrderAggregate.Specifications;

public class GetOrdersByCustomerIdSpecification : Specification<Order>
{
    public GetOrdersByCustomerIdSpecification(CustomerId customerId)
        : base(o => o.CustomerId == customerId)
    {
        AddInclude(order => order.Include(o => o.OrderItems));
    }
}
