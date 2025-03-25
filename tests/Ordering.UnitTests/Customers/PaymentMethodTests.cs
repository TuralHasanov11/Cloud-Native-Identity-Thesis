using Ordering.Core.CustomerAggregate;
using Ordering.Core.Exceptions;

namespace Ordering.UnitTests.Customers;

public class PaymentMethodTests
{
    [Fact]
    public void Constructor_ShouldInitializePaymentMethod()
    {
        // Arrange
        var cardTypeId = 1;
        var alias = "Visa";
        var cardNumber = "1234567890123456";
        var securityNumber = "123";
        var cardHolderName = "John Doe";
        var expirationDate = DateTime.UtcNow.AddYears(1);

        // Act
        var paymentMethod = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expirationDate);

        // Assert
        Assert.Equal(cardTypeId, paymentMethod.CardTypeId);
        Assert.Equal(alias, paymentMethod.Alias);
        Assert.Equal(cardNumber, paymentMethod.CardNumber);
        Assert.Equal(securityNumber, paymentMethod.SecurityNumber);
        Assert.Equal(cardHolderName, paymentMethod.CardHolderName);
        Assert.Equal(expirationDate, paymentMethod.ExpirationDate);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenCardNumberIsNull()
    {
        // Arrange
        var cardTypeId = 1;
        var alias = "Visa";
        var securityNumber = "123";
        var cardHolderName = "John Doe";
        var expirationDate = DateTime.UtcNow.AddYears(1);

        // Act & Assert
        var exception = Assert.Throws<OrderingDomainException>(() => new PaymentMethod(cardTypeId, alias, null, securityNumber, cardHolderName, expirationDate));
        Assert.Equal("cardNumber", exception.Message);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenCardNumberIsEmpty()
    {
        // Arrange
        var cardTypeId = 1;
        var alias = "Visa";
        var securityNumber = "123";
        var cardHolderName = "John Doe";
        var expirationDate = DateTime.UtcNow.AddYears(1);

        // Act & Assert
        var exception = Assert.Throws<OrderingDomainException>(() => new PaymentMethod(cardTypeId, alias, string.Empty, securityNumber, cardHolderName, expirationDate));
        Assert.Equal("cardNumber", exception.Message);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenSecurityNumberIsNull()
    {
        // Arrange
        var cardTypeId = 1;
        var alias = "Visa";
        var cardNumber = "1234567890123456";
        var cardHolderName = "John Doe";
        var expirationDate = DateTime.UtcNow.AddYears(1);

        // Act & Assert
        var exception = Assert.Throws<OrderingDomainException>(() => new PaymentMethod(cardTypeId, alias, cardNumber, null, cardHolderName, expirationDate));
        Assert.Equal("securityNumber", exception.Message);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenSecurityNumberIsEmpty()
    {
        // Arrange
        var cardTypeId = 1;
        var alias = "Visa";
        var cardNumber = "1234567890123456";
        var cardHolderName = "John Doe";
        var expirationDate = DateTime.UtcNow.AddYears(1);

        // Act & Assert
        var exception = Assert.Throws<OrderingDomainException>(() => new PaymentMethod(cardTypeId, alias, cardNumber, string.Empty, cardHolderName, expirationDate));
        Assert.Equal("securityNumber", exception.Message);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenCardHolderNameIsNull()
    {
        // Arrange
        var cardTypeId = 1;
        var alias = "Visa";
        var cardNumber = "1234567890123456";
        var securityNumber = "123";
        var expirationDate = DateTime.UtcNow.AddYears(1);

        // Act & Assert
        var exception = Assert.Throws<OrderingDomainException>(() => new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, null, expirationDate));
        Assert.Equal("cardHolderName", exception.Message);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenCardHolderNameIsEmpty()
    {
        // Arrange
        var cardTypeId = 1;
        var alias = "Visa";
        var cardNumber = "1234567890123456";
        var securityNumber = "123";
        var expirationDate = DateTime.UtcNow.AddYears(1);

        // Act & Assert
        var exception = Assert.Throws<OrderingDomainException>(() => new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, string.Empty, expirationDate));
        Assert.Equal("cardHolderName", exception.Message);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenExpirationDateIsInThePast()
    {
        // Arrange
        var cardTypeId = 1;
        var alias = "Visa";
        var cardNumber = "1234567890123456";
        var securityNumber = "123";
        var cardHolderName = "John Doe";
        var expirationDate = DateTime.UtcNow.AddYears(-1);

        // Act & Assert
        var exception = Assert.Throws<OrderingDomainException>(() => new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expirationDate));
        Assert.Equal("expirationDate", exception.Message);
    }

    [Fact]
    public void IsEqualTo_ShouldReturnTrue_WhenPaymentMethodsAreEqual()
    {
        // Arrange
        var cardTypeId = 1;
        var alias = "Visa";
        var cardNumber = "1234567890123456";
        var securityNumber = "123";
        var cardHolderName = "John Doe";
        var expirationDate = DateTime.UtcNow.AddYears(1);

        var paymentMethod = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expirationDate);

        // Act
        var result = paymentMethod.IsEqualTo(cardTypeId, cardNumber, expirationDate);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsEqualTo_ShouldReturnFalse_WhenPaymentMethodsAreNotEqual()
    {
        // Arrange
        var cardTypeId = 1;
        var alias = "Visa";
        var cardNumber = "1234567890123456";
        var securityNumber = "123";
        var cardHolderName = "John Doe";
        var expirationDate = DateTime.UtcNow.AddYears(1);

        var paymentMethod = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expirationDate);

        // Act
        var result = paymentMethod.IsEqualTo(cardTypeId, "6543210987654321", expirationDate);

        // Assert
        Assert.False(result);
    }
}
