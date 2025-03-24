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

    public ICollection<PaymentMethod> PaymentMethods { get; }

    public PaymentMethod VerifyOrAddPaymentMethod(
        int cardTypeId,
        string alias,
        string cardNumber,
        string securityNumber,
        string cardHolderName,
        DateTime expiration,
        OrderId orderId)
    {
        var existingPayment = PaymentMethods
            .SingleOrDefault(p => p.IsEqualTo(cardTypeId, cardNumber, expiration));

        if (existingPayment != null)
        {
            AddDomainEvent(new CustomerAndPaymentMethodVerifiedDomainEvent(this, existingPayment, orderId, DateTime.UtcNow));

            return existingPayment;
        }

        var payment = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration);

        PaymentMethods.Add(payment);

        AddDomainEvent(new CustomerAndPaymentMethodVerifiedDomainEvent(this, payment, orderId, DateTime.UtcNow));

        return payment;
    }

    public static Customer Create(IdentityId identityId, string name)
    {
        return new Customer(identityId, name);
    }
}
