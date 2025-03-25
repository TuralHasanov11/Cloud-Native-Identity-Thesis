using Ordering.Core.CustomerAggregate;
using Ordering.Core.Exceptions;

namespace Ordering.UnitTests.Customers;

public class CardTypeTests
{
    [Fact]
    public void Constructor_ShouldInitializeCardType()
    {
        // Arrange
        var name = "Visa";

        // Act
        var cardType = new CardType(name);

        // Assert
        Assert.Equal(name, cardType.Name);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenNameIsNull()
    {
        // Act & Assert
        var exception = Assert.Throws<OrderingDomainException>(() => new CardType(null));
        Assert.Equal("name", exception.Message);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenNameIsEmpty()
    {
        // Act & Assert
        var exception = Assert.Throws<OrderingDomainException>(() => new CardType(string.Empty));
        Assert.Equal("name", exception.Message);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenNameIsWhitespace()
    {
        // Act & Assert
        var exception = Assert.Throws<OrderingDomainException>(() => new CardType(" "));
        Assert.Equal("name", exception.Message);
    }
}
