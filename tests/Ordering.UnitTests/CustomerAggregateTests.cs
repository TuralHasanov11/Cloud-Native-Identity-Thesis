using Ordering.Core.CustomerAggregate;
using Ordering.Core.Exceptions;

namespace Ordering.UnitTests;

public class CustomerAggregateTests
{
    [Fact]
    public void CreateMethod_CreatesCustomer()
    {
        var identity = new IdentityId(IdentityExtensions.GenerateId());
        const string name = "fakeUser";

        var fakeCustomerItem = Customer.Create(identity, name);

        Assert.NotNull(fakeCustomerItem);
    }

    [Fact]
    public void CreateCustomer_WithEmptyValues_ThrowsException()
    {
        var identity = new IdentityId(string.Empty);
        var name = string.Empty;

        Assert.Throws<ArgumentNullException>(() => Customer.Create(identity, name));
    }

    [Fact]
    public void AddPayment_Method_Succeeds()
    {
        const int cardTypeId = 1;
        const string alias = "fakeAlias";
        const string cardNumber = "124";
        const string securityNumber = "1234";
        const string cardHolderName = "FakeHolderNAme";
        var expiration = DateTime.UtcNow.AddYears(1);
        var orderId = new OrderId(Guid.CreateVersion7());
        const string name = "fakeUser";
        var identity = new IdentityId(string.Empty);
        var fakeCustomerItem = Customer.Create(identity, name);

        var result = fakeCustomerItem.VerifyOrAddPaymentMethod(
            cardTypeId,
            alias,
            cardNumber,
            securityNumber,
            cardHolderName,
            expiration,
            orderId);

        Assert.NotNull(result);
    }

    [Fact]
    public void CreatePayment_Method_Succeeds()
    {
        const int cardTypeId = 1;
        const string alias = "fakeAlias";
        const string cardNumber = "124";
        const string securityNumber = "1234";
        const string cardHolderName = "FakeHolderNAme";
        var expiration = DateTime.UtcNow.AddYears(1);
        var fakePaymentMethod = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration);

        Assert.NotNull(fakePaymentMethod);
    }

    [Fact]
    public void CreatePayment_Method_Expiration_Fails()
    {
        const int cardTypeId = 1;
        const string alias = "fakeAlias";
        const string cardNumber = "124";
        const string securityNumber = "1234";
        const string cardHolderName = "FakeHolderNAme";
        var expiration = DateTime.UtcNow.AddYears(-1);

        Assert.Throws<OrderingDomainException>(
            () => new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration));
    }

    [Fact]
    public void Payment_Method_IsEqualTo()
    {
        const int cardTypeId = 1;
        const string alias = "fakeAlias";
        const string cardNumber = "124";
        const string securityNumber = "1234";
        const string cardHolderName = "FakeHolderNAme";
        var expiration = DateTime.UtcNow.AddYears(1);

        var fakePaymentMethod = new PaymentMethod(
            cardTypeId,
            alias,
            cardNumber,
            securityNumber,
            cardHolderName,
            expiration);

        var result = fakePaymentMethod.IsEqualTo(cardTypeId, cardNumber, expiration);

        Assert.True(result);
    }

    [Fact]
    public void Add_New_PaymentMethod_Raises_New_Event()
    {
        const string alias = "fakeAlias";
        OrderId orderId = new(Guid.CreateVersion7());
        const int cardTypeId = 5;
        const string cardNumber = "12";
        const string cardSecurityNumber = "123";
        const string cardHolderName = "FakeName";
        DateTime cardExpiration = DateTime.UtcNow.AddYears(1);
        const string name = "fakeUser";

        var fakeCustomer = Customer.Create(new IdentityId(IdentityExtensions.GenerateId()), name);
        fakeCustomer.VerifyOrAddPaymentMethod(
            cardTypeId,
            alias,
            cardNumber,
            cardSecurityNumber,
            cardHolderName,
            cardExpiration,
            orderId);

        Assert.Single(fakeCustomer.DomainEvents);
    }
}
