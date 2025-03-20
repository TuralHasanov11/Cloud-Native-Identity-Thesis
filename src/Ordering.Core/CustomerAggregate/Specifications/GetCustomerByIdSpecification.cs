namespace Ordering.Core.CustomerAggregate.Specifications;

public sealed class GetCustomerByIdSpecification : Specification<Customer>
{
    public GetCustomerByIdSpecification(IdentityId identityId)
        : base(c => c.IdentityId == identityId)
    {
    }

    public GetCustomerByIdSpecification(CustomerId id)
        : base(c => c.Id == id)
    {
    }
}
