using Ordering.Core.CustomerAggregate;

namespace Ordering.UnitTests.Customers;

public class CardTypeTests
{
    [Fact]
    public void Constructor_ShouldInitializeCardType()
    {
        // Arrange
        var name = "Visa";

        // Act
        var cardType = CardType.Create(name);

        // Assert
        Assert.Equal(name, cardType.Name);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenNameIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => CardType.Create(null));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenNameIsEmpty()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => CardType.Create(string.Empty));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenNameIsWhitespace()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => CardType.Create(" "));
    }
}
