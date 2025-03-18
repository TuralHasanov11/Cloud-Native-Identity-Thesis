namespace Ordering.Core.CustomerAggregate.Specifications;

public sealed class GetCustomerByIdSpecification(CustomerId Id) : Specification<Customer>(c => c.Id == Id);
