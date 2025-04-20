using Ordering.Core.CustomerAggregate;
using Ordering.Core.CustomerAggregate.Events;

namespace Ordering.UnitTests.Customers;

public class CustomerTests
{
    [Fact]
    public void Create_ShouldInitializeCustomer()
    {
        // Arrange
        var identityId = new IdentityId("id");
        var name = "John Doe";

        // Act
        var customer = Customer.Create(identityId, name);

        // Assert
        Assert.Equal(identityId, customer.IdentityId);
        Assert.Equal(name, customer.Name);
        Assert.Empty(customer.PaymentMethods);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenNameIsNull()
    {
        // Arrange
        var identityId = new IdentityId("id");

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Customer.Create(identityId, null));
        Assert.Equal("Value cannot be null. (Parameter 'name')", exception.Message);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenNameIsEmpty()
    {
        // Arrange
        var identityId = new IdentityId("id");

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Customer.Create(identityId, string.Empty));
        Assert.Equal("Value cannot be null. (Parameter 'name')", exception.Message);
    }

    [Fact]
    public void VerifyOrAddPaymentMethod_ShouldAddNewPaymentMethod()
    {
        // Arrange
        var customer = Customer.Create(new IdentityId("id"), "John Doe");
        var cardTypeId = 1;
        var alias = "Visa";
        var orderId = new OrderId(Guid.CreateVersion7());

        // Act
        var paymentMethod = customer.VerifyOrAddPaymentMethod(cardTypeId, alias, orderId);

        // Assert
        Assert.Single(customer.PaymentMethods);
        Assert.Equal(paymentMethod, customer.PaymentMethods.First());
    }

    [Fact]
    public void VerifyOrAddPaymentMethod_ShouldReturnExistingPaymentMethod()
    {
        // Arrange
        var customer = Customer.Create(new IdentityId("id"), "John Doe");
        var cardTypeId = 1;
        var alias = "Visa";
        var orderId = new OrderId(Guid.CreateVersion7());

        var existingPaymentMethod = customer.VerifyOrAddPaymentMethod(cardTypeId, alias,orderId);

        // Act
        var paymentMethod = customer.VerifyOrAddPaymentMethod(cardTypeId, alias, orderId);

        // Assert
        Assert.Single(customer.PaymentMethods);
        Assert.Equal(existingPaymentMethod, paymentMethod);
    }

    [Fact]
    public void VerifyOrAddPaymentMethod_ShouldAddDomainEvent()
    {
        // Arrange
        var customer = Customer.Create(new IdentityId("id"), "John Doe");
        var cardTypeId = 1;
        var alias = "Visa";
        var orderId = new OrderId(Guid.CreateVersion7());

        // Act
        var paymentMethod = customer.VerifyOrAddPaymentMethod(cardTypeId, alias, orderId);

        // Assert
        var domainEvent = customer.DomainEvents.FirstOrDefault() as CustomerAndPaymentMethodVerifiedDomainEvent;
        Assert.NotNull(domainEvent);
        Assert.Equal(customer, domainEvent.Customer);
        Assert.Equal(paymentMethod, domainEvent.PaymentMethod);
        Assert.Equal(orderId, domainEvent.OrderId);
    }
}
