using Ordering.Core.CustomerAggregate;

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
        var orderId = new OrderId(Guid.CreateVersion7());
        const string name = "fakeUser";
        var identity = new IdentityId(string.Empty);
        var fakeCustomerItem = Customer.Create(identity, name);

        var result = fakeCustomerItem.VerifyOrAddPaymentMethod(
            cardTypeId,
            alias,
            orderId);

        Assert.NotNull(result);
    }

    [Fact]
    public void CreatePayment_Method_Succeeds()
    {
        const int cardTypeId = 1;
        const string alias = "fakeAlias";

        var fakePaymentMethod = new PaymentMethod(cardTypeId, alias);

        Assert.NotNull(fakePaymentMethod);
    }

    [Fact]
    public void Payment_Method_IsEqualTo()
    {
        const int cardTypeId = 1;
        const string alias = "fakeAlias";

        var fakePaymentMethod = new PaymentMethod(
            cardTypeId,
            alias);

        var result = fakePaymentMethod.IsEqualTo(cardTypeId);

        Assert.True(result);
    }

    [Fact]
    public void Add_New_PaymentMethod_Raises_New_Event()
    {
        const string alias = "fakeAlias";
        OrderId orderId = new(Guid.CreateVersion7());
        const int cardTypeId = 5;
        const string name = "fakeUser";

        var fakeCustomer = Customer.Create(new IdentityId(IdentityExtensions.GenerateId()), name);
        fakeCustomer.VerifyOrAddPaymentMethod(
            cardTypeId,
            alias,
            orderId);

        Assert.Single(fakeCustomer.DomainEvents);
    }
}
