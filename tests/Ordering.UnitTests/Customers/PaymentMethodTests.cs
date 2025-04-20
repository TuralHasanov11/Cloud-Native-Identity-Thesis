using Ordering.Core.CustomerAggregate;

namespace Ordering.UnitTests.Customers;

public class PaymentMethodTests
{
    [Fact]
    public void Constructor_ShouldInitializePaymentMethod()
    {
        // Arrange
        var cardTypeId = 1;
        var alias = "Visa";

        // Act
        var paymentMethod = new PaymentMethod(cardTypeId, alias);

        // Assert
        Assert.Equal(cardTypeId, paymentMethod.CardTypeId);
        Assert.Equal(alias, paymentMethod.Alias);
    }

    [Fact]
    public void IsEqualTo_ShouldReturnTrue_WhenPaymentMethodsAreEqual()
    {
        // Arrange
        var cardTypeId = 1;
        var alias = "Visa";

        var paymentMethod = new PaymentMethod(cardTypeId, alias);

        // Act
        var result = paymentMethod.IsEqualTo(cardTypeId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsEqualTo_ShouldReturnFalse_WhenPaymentMethodsAreNotEqual()
    {
        // Arrange
        var cardTypeId = 1;
        var alias = "Visa";

        var paymentMethod = new PaymentMethod(cardTypeId, alias);

        // Act
        var result = paymentMethod.IsEqualTo(2);

        // Assert
        Assert.False(result);
    }
}
