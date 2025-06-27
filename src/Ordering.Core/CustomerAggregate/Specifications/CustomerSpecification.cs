using Microsoft.EntityFrameworkCore;

namespace Ordering.Core.CustomerAggregate.Specifications;

public sealed class CustomerSpecification : Specification<Customer>
{
    public CustomerSpecification()
        : base()
    {
    }

    public CustomerSpecification(IdentityId identityId)
        : base(c => c.IdentityId == identityId)
    {
        AddInclude(customer => customer.Include(c => c.PaymentMethods));
    }

    public CustomerSpecification(CustomerId id)
        : base(c => c.Id == id)
    {
        AddInclude(customer => customer.Include(c => c.PaymentMethods));
    }
}
