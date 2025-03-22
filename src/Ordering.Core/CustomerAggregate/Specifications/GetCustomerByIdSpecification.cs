using Microsoft.EntityFrameworkCore;

namespace Ordering.Core.CustomerAggregate.Specifications;

public sealed class GetCustomerByIdSpecification : Specification<Customer>
{
    public GetCustomerByIdSpecification(IdentityId identityId)
        : base(c => c.IdentityId == identityId)
    {
        AddInclude(customer => customer.Include(c => c.PaymentMethods));
    }

    public GetCustomerByIdSpecification(CustomerId id)
        : base(c => c.Id == id)
    {
        AddInclude(customer => customer.Include(c => c.PaymentMethods));
    }
}
