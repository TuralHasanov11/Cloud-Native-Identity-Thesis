namespace Ordering.Core.CustomerAggregate;

public sealed class Customer : EntityBase<CustomerId>, IAggregateRoot
{
    private Customer(IdentityId identityId, string name)
        : base(new CustomerId(Guid.CreateVersion7()))
    {
        IdentityId = identityId;
        Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
    }

    public IdentityId IdentityId { get; }

    public string Name { get; }

    public ICollection<PaymentMethod> PaymentMethods { get; } = [];

    public PaymentMethod VerifyOrAddPaymentMethod(
        int cardTypeId,
        string alias,
        OrderId orderId)
    {
        var existingPayment = PaymentMethods
            .FirstOrDefault(p => p.IsEqualTo(cardTypeId));

        if (existingPayment != null)
        {
            AddDomainEvent(new CustomerAndPaymentMethodVerifiedDomainEvent(this, existingPayment, orderId, DateTime.UtcNow));

            return existingPayment;
        }

        var payment = new PaymentMethod(cardTypeId, alias);

        PaymentMethods.Add(payment);

        AddDomainEvent(new CustomerAndPaymentMethodVerifiedDomainEvent(this, payment, orderId, DateTime.UtcNow));

        return payment;
    }

    public static Customer Create(IdentityId identityId, string name)
    {
        return new Customer(identityId, name);
    }
}
