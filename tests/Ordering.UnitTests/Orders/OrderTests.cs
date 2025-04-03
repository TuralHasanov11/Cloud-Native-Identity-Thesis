using Ordering.Core.CustomerAggregate;
using Ordering.Core.Exceptions;

namespace Ordering.UnitTests.Orders;

public class OrderTests
{
    [Fact]
    public void NewDraft_ShouldInitializeOrderAsDraft()
    {
        // Act
        var order = Order.NewDraft();

        // Assert
        Assert.True(order.IsDraft);
        Assert.Equal(OrderStatus.Default, order.OrderStatus);
    }

    [Fact]
    public void Constructor_ShouldInitializeOrder()
    {
        // Arrange
        var userId = new IdentityId(IdentityExtensions.GenerateId());
        var userName = "John Doe";
        var address = new Address("Street", "City", "State", "Country", "ZipCode");
        var cardTypeId = 1;
        var cardNumber = "1234567890123456";
        var cardSecurityNumber = "123";
        var cardHolderName = "John Doe";
        var cardExpiration = DateTime.UtcNow.AddYears(1);
        var customerId = new CustomerId(Guid.CreateVersion7());
        var alias = "Visa";
        var securityNumber = "123";

        var expirationDate = DateTime.UtcNow.AddYears(1);

        // Act
        var paymentMethod = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expirationDate);

        // Act
        var order = new Order(userId, userName, address, cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration, customerId, paymentMethod.Id);

        // Assert
        Assert.Equal(OrderStatus.Submitted, order.OrderStatus);
        Assert.Equal(address, order.Address);
    }

    [Fact]
    public void AddOrderItem_ShouldAddNewItem()
    {
        // Arrange
        var userId = new IdentityId(IdentityExtensions.GenerateId());
        const string userName = "John Doe";
        var address = new Address("Street", "City", "State", "Country", "ZipCode");
        const int cardTypeId = 1;
        const string cardNumber = "1234567890123456";
        const string cardSecurityNumber = "123";
        const string cardHolderName = "John Doe";
        var cardExpiration = DateTime.UtcNow.AddYears(1);
        var customerId = new CustomerId(Guid.CreateVersion7());
        const string alias = "Visa";
        const string securityNumber = "123";

        var expirationDate = DateTime.UtcNow.AddYears(1);

        var paymentMethod = new PaymentMethod(
            cardTypeId,
            alias,
            cardNumber,
            securityNumber,
            cardHolderName,
            expirationDate);

        var order = new Order(
            userId,
            userName,
            address,
            cardTypeId,
            cardNumber,
            cardSecurityNumber,
            cardHolderName,
            cardExpiration,
            customerId,
            paymentMethod.Id);

        var productId = Guid.CreateVersion7();
        var productName = "Product1";
        var unitPrice = 10.0m;
        var discount = 1.0m;
        var pictureUrl = new Uri("http://example.com/picture.jpg");
        var units = 2;

        // Act
        order.AddOrderItem(productId, productName, unitPrice, discount, pictureUrl, units);

        // Assert
        Assert.Single(order.OrderItems);
        var orderItem = order.OrderItems.First();
        Assert.Equal(productId, orderItem.ProductId);
        Assert.Equal(productName, orderItem.ProductName);
        Assert.Equal(unitPrice, orderItem.UnitPrice);
        Assert.Equal(discount, orderItem.Discount);
        Assert.Equal(units, orderItem.Units);
        Assert.Equal(pictureUrl, orderItem.PictureUrl);
    }

    [Fact]
    public void AddOrderItem_ShouldUpdateExistingItem()
    {
        // Arrange
        var userId = new IdentityId(IdentityExtensions.GenerateId());
        const string userName = "John Doe";
        var address = new Address("Street", "City", "State", "Country", "ZipCode");
        const int cardTypeId = 1;
        const string cardNumber = "1234567890123456";
        const string cardSecurityNumber = "123";
        const string cardHolderName = "John Doe";
        var cardExpiration = DateTime.UtcNow.AddYears(1);
        var customerId = new CustomerId(Guid.CreateVersion7());
        const string alias = "Visa";
        const string securityNumber = "123";

        var expirationDate = DateTime.UtcNow.AddYears(1);

        var paymentMethod = new PaymentMethod(
            cardTypeId,
            alias,
            cardNumber,
            securityNumber,
            cardHolderName,
            expirationDate);

        var order = new Order(
            userId,
            userName,
            address,
            cardTypeId,
            cardNumber,
            cardSecurityNumber,
            cardHolderName,
            cardExpiration,
            customerId,
            paymentMethod.Id);

        var productId = Guid.CreateVersion7();
        var productName = "Product1";
        var unitPrice = 10.0m;
        var discount = 1.0m;
        var pictureUrl = new Uri("http://example.com/picture.jpg");
        var units = 2;

        order.AddOrderItem(productId, productName, unitPrice, discount, pictureUrl, units);

        // Act
        order.AddOrderItem(productId, productName, unitPrice, discount, pictureUrl, units);

        // Assert
        Assert.Single(order.OrderItems);
        var orderItem = order.OrderItems.First();
        Assert.Equal(units * 2, orderItem.Units);
    }

    [Fact]
    public void VerifyPayment_ShouldSetCustomerIdAndPaymentMethodId()
    {
        // Arrange
        var userId = new IdentityId(IdentityExtensions.GenerateId());
        const string userName = "John Doe";
        var address = new Address("Street", "City", "State", "Country", "ZipCode");
        const int cardTypeId = 1;
        const string cardNumber = "1234567890123456";
        const string cardSecurityNumber = "123";
        const string cardHolderName = "John Doe";
        var cardExpiration = DateTime.UtcNow.AddYears(1);
        const string alias = "Visa";
        const string securityNumber = "123";

        var expirationDate = DateTime.UtcNow.AddYears(1);

        var paymentMethod = new PaymentMethod(
            cardTypeId,
            alias,
            cardNumber,
            securityNumber,
            cardHolderName,
            expirationDate);

        var order = new Order(
            userId,
            userName,
            address,
            cardTypeId,
            cardNumber,
            cardSecurityNumber,
            cardHolderName,
            cardExpiration,
            new CustomerId(Guid.CreateVersion7()),
            paymentMethod.Id);

        var customerId = new CustomerId(Guid.CreateVersion7());
        var paymentMethodId = new PaymentMethodId(Guid.CreateVersion7());

        // Act
        order.VerifyPayment(customerId, paymentMethodId);

        // Assert
        Assert.Equal(customerId, order.CustomerId);
        Assert.Equal(paymentMethodId, order.PaymentMethodId);
    }

    [Fact]
    public void SetAwaitingValidationStatus_ShouldChangeStatusToAwaitingValidation()
    {
        // Arrange
        var userId = new IdentityId(IdentityExtensions.GenerateId());
        const string userName = "John Doe";
        var address = new Address("Street", "City", "State", "Country", "ZipCode");
        const int cardTypeId = 1;
        const string cardNumber = "1234567890123456";
        const string cardSecurityNumber = "123";
        const string cardHolderName = "John Doe";
        var cardExpiration = DateTime.UtcNow.AddYears(1);
        const string alias = "Visa";
        const string securityNumber = "123";

        var expirationDate = DateTime.UtcNow.AddYears(1);

        var paymentMethod = new PaymentMethod(
            cardTypeId,
            alias,
            cardNumber,
            securityNumber,
            cardHolderName,
            expirationDate);

        var order = new Order(
            userId,
            userName,
            address,
            cardTypeId,
            cardNumber,
            cardSecurityNumber,
            cardHolderName,
            cardExpiration,
            new CustomerId(Guid.CreateVersion7()),
            paymentMethod.Id);

        order.SetAwaitingValidationStatus();

        // Act
        order.SetAwaitingValidationStatus();

        // Assert
        Assert.Equal(OrderStatus.AwaitingValidation, order.OrderStatus);
    }

    [Fact]
    public void ConfirmStock_ShouldChangeStatusToStockConfirmed()
    {
        // Arrange
        // Arrange
        var userId = new IdentityId(IdentityExtensions.GenerateId());
        const string userName = "John Doe";
        var address = new Address("Street", "City", "State", "Country", "ZipCode");
        const int cardTypeId = 1;
        const string cardNumber = "1234567890123456";
        const string cardSecurityNumber = "123";
        const string cardHolderName = "John Doe";
        var cardExpiration = DateTime.UtcNow.AddYears(1);
        var customerId = new CustomerId(Guid.CreateVersion7());
        const string alias = "Visa";
        const string securityNumber = "123";

        var expirationDate = DateTime.UtcNow.AddYears(1);

        var paymentMethod = new PaymentMethod(
            cardTypeId,
            alias,
            cardNumber,
            securityNumber,
            cardHolderName,
            expirationDate);

        var order = new Order(
            userId,
            userName,
            address,
            cardTypeId,
            cardNumber,
            cardSecurityNumber,
            cardHolderName,
            cardExpiration,
            customerId,
            paymentMethod.Id);

        order.SetAwaitingValidationStatus();

        // Act
        order.ConfirmStock();

        // Assert
        Assert.Equal(OrderStatus.StockConfirmed, order.OrderStatus);
        Assert.Equal("All the items were confirmed with available stock.", order.Description);
    }

    [Fact]
    public void Pay_ShouldChangeStatusToPaid()
    {
        // Arrange
        // Arrange
        var userId = new IdentityId(IdentityExtensions.GenerateId());
        const string userName = "John Doe";
        var address = new Address("Street", "City", "State", "Country", "ZipCode");
        const int cardTypeId = 1;
        const string cardNumber = "1234567890123456";
        const string cardSecurityNumber = "123";
        const string cardHolderName = "John Doe";
        var cardExpiration = DateTime.UtcNow.AddYears(1);
        var customerId = new CustomerId(Guid.CreateVersion7());
        const string alias = "Visa";
        const string securityNumber = "123";

        var expirationDate = DateTime.UtcNow.AddYears(1);

        var paymentMethod = new PaymentMethod(
            cardTypeId,
            alias,
            cardNumber,
            securityNumber,
            cardHolderName,
            expirationDate);

        var order = new Order(
            userId,
            userName,
            address,
            cardTypeId,
            cardNumber,
            cardSecurityNumber,
            cardHolderName,
            cardExpiration,
            customerId,
            paymentMethod.Id);

        order.SetAwaitingValidationStatus();
        order.ConfirmStock();

        // Act
        order.Pay();

        // Assert
        Assert.Equal(OrderStatus.Paid, order.OrderStatus);
        Assert.Equal("The payment was performed at a simulated \"American Bank checking bank account ending on XX35071\"", order.Description);
    }

    [Fact]
    public void Ship_ShouldChangeStatusToShipped()
    {
        // Arrange
        // Arrange
        var userId = new IdentityId(IdentityExtensions.GenerateId());
        const string userName = "John Doe";
        var address = new Address("Street", "City", "State", "Country", "ZipCode");
        const int cardTypeId = 1;
        const string cardNumber = "1234567890123456";
        const string cardSecurityNumber = "123";
        const string cardHolderName = "John Doe";
        var cardExpiration = DateTime.UtcNow.AddYears(1);
        var customerId = new CustomerId(Guid.CreateVersion7());
        const string alias = "Visa";
        const string securityNumber = "123";

        var expirationDate = DateTime.UtcNow.AddYears(1);

        var paymentMethod = new PaymentMethod(
            cardTypeId,
            alias,
            cardNumber,
            securityNumber,
            cardHolderName,
            expirationDate);

        var order = new Order(
            userId,
            userName,
            address,
            cardTypeId,
            cardNumber,
            cardSecurityNumber,
            cardHolderName,
            cardExpiration,
            customerId,
            paymentMethod.Id);

        order.SetAwaitingValidationStatus();
        order.ConfirmStock();
        order.Pay();

        // Act
        order.Ship();

        // Assert
        Assert.Equal(OrderStatus.Shipped, order.OrderStatus);
        Assert.Equal("The order was shipped.", order.Description);
    }

    [Fact]
    public void Cancel_ShouldChangeStatusToCancelled()
    {
        // Arrange
        // Arrange
        var userId = new IdentityId(IdentityExtensions.GenerateId());
        const string userName = "John Doe";
        var address = new Address("Street", "City", "State", "Country", "ZipCode");
        const int cardTypeId = 1;
        const string cardNumber = "1234567890123456";
        const string cardSecurityNumber = "123";
        const string cardHolderName = "John Doe";
        var cardExpiration = DateTime.UtcNow.AddYears(1);
        var customerId = new CustomerId(Guid.CreateVersion7());
        const string alias = "Visa";
        const string securityNumber = "123";

        var expirationDate = DateTime.UtcNow.AddYears(1);

        var paymentMethod = new PaymentMethod(
            cardTypeId,
            alias,
            cardNumber,
            securityNumber,
            cardHolderName,
            expirationDate);

        var order = new Order(
            userId,
            userName,
            address,
            cardTypeId,
            cardNumber,
            cardSecurityNumber,
            cardHolderName,
            cardExpiration,
            customerId,
            paymentMethod.Id);

        // Act
        order.Cancel();

        // Assert
        Assert.Equal(OrderStatus.Cancelled, order.OrderStatus);
        Assert.Equal("The order was cancelled.", order.Description);
    }

    [Fact]
    public void Cancel_ShouldChangeStatusToCancelled_WhenStockRejected()
    {
        // Arrange
        // Arrange
        var userId = new IdentityId(IdentityExtensions.GenerateId());
        const string userName = "John Doe";
        var address = new Address("Street", "City", "State", "Country", "ZipCode");
        const int cardTypeId = 1;
        const string cardNumber = "1234567890123456";
        const string cardSecurityNumber = "123";
        const string cardHolderName = "John Doe";
        var cardExpiration = DateTime.UtcNow.AddYears(1);
        var customerId = new CustomerId(Guid.CreateVersion7());
        const string alias = "Visa";
        const string securityNumber = "123";

        var expirationDate = DateTime.UtcNow.AddYears(1);

        var paymentMethod = new PaymentMethod(
            cardTypeId,
            alias,
            cardNumber,
            securityNumber,
            cardHolderName,
            expirationDate);

        var order = new Order(
            userId,
            userName,
            address,
            cardTypeId,
            cardNumber,
            cardSecurityNumber,
            cardHolderName,
            cardExpiration,
            customerId,
            paymentMethod.Id);

        order.SetAwaitingValidationStatus();
        var productId = Guid.CreateVersion7();
        order.AddOrderItem(productId, "Product1", 10.0m, 1.0m, new Uri("http://example.com/picture.jpg"), 1);

        // Act
        order.Cancel(new[] { productId });

        // Assert
        Assert.Equal(OrderStatus.Cancelled, order.OrderStatus);
        Assert.Equal("The product items don't have stock: (Product1).", order.Description);
    }

    [Fact]
    public void GetTotal_ShouldReturnTotalAmount()
    {
        // Arrange
        // Arrange
        var userId = new IdentityId(IdentityExtensions.GenerateId());
        const string userName = "John Doe";
        var address = new Address("Street", "City", "State", "Country", "ZipCode");
        const int cardTypeId = 1;
        const string cardNumber = "1234567890123456";
        const string cardSecurityNumber = "123";
        const string cardHolderName = "John Doe";
        var cardExpiration = DateTime.UtcNow.AddYears(1);
        var customerId = new CustomerId(Guid.CreateVersion7());
        const string alias = "Visa";
        const string securityNumber = "123";

        var expirationDate = DateTime.UtcNow.AddYears(1);

        var paymentMethod = new PaymentMethod(
            cardTypeId,
            alias,
            cardNumber,
            securityNumber,
            cardHolderName,
            expirationDate);

        var order = new Order(
            userId,
            userName,
            address,
            cardTypeId,
            cardNumber,
            cardSecurityNumber,
            cardHolderName,
            cardExpiration,
            customerId,
            paymentMethod.Id);

        order.AddOrderItem(Guid.CreateVersion7(), "Product1", 10.0m, 1.0m, new Uri("http://example.com/picture.jpg"), 2);
        order.AddOrderItem(Guid.CreateVersion7(), "Product2", 20.0m, 2.0m, new Uri("http://example.com/picture.jpg"), 1);

        // Act
        var total = order.GetTotal();

        // Assert
        Assert.Equal(36.0m, total);
    }
}

public class OrderItemTests
{
    [Fact]
    public void Constructor_ShouldInitializeOrderItem()
    {
        // Arrange
        var orderId = new OrderId(Guid.CreateVersion7());
        var productId = Guid.CreateVersion7();
        var productName = "Product1";
        var unitPrice = 10.0m;
        var discount = 1.0m;
        var pictureUrl = new Uri("http://example.com/picture.jpg");
        var units = 2;

        // Act
        var orderItem = new OrderItem(orderId, productId, productName, unitPrice, discount, pictureUrl, units);

        // Assert
        Assert.Equal(orderId, orderItem.OrderId);
        Assert.Equal(productId, orderItem.ProductId);
        Assert.Equal(productName, orderItem.ProductName);
        Assert.Equal(unitPrice, orderItem.UnitPrice);
        Assert.Equal(discount, orderItem.Discount);
        Assert.Equal(units, orderItem.Units);
        Assert.Equal(pictureUrl, orderItem.PictureUrl);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenUnitsIsZero()
    {
        // Arrange
        var orderId = new OrderId(Guid.CreateVersion7());
        var productId = Guid.CreateVersion7();
        var productName = "Product1";
        var unitPrice = 10.0m;
        var discount = 1.0m;
        var pictureUrl = new Uri("http://example.com/picture.jpg");

        // Act & Assert
        var exception = Assert.Throws<OrderingDomainException>(() => new OrderItem(orderId, productId, productName, unitPrice, discount, pictureUrl, 0));
        Assert.Equal("Invalid number of units", exception.Message);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenTotalIsLowerThanDiscount()
    {
        // Arrange
        var orderId = new OrderId(Guid.CreateVersion7());
        var productId = Guid.CreateVersion7();
        var productName = "Product1";
        var unitPrice = 10.0m;
        var discount = 20.0m;
        var pictureUrl = new Uri("http://example.com/picture.jpg");
        var units = 1;

        // Act & Assert
        var exception = Assert.Throws<OrderingDomainException>(() => new OrderItem(orderId, productId, productName, unitPrice, discount, pictureUrl, units));
        Assert.Equal("The total of order item is lower than applied discount", exception.Message);
    }

    [Fact]
    public void SetNewDiscount_ShouldUpdateDiscount()
    {
        // Arrange
        var orderId = new OrderId(Guid.CreateVersion7());
        var productId = Guid.CreateVersion7();
        var productName = "Product1";
        var unitPrice = 10.0m;
        var discount = 1.0m;
        var pictureUrl = new Uri("http://example.com/picture.jpg");
        var units = 2;

        var orderItem = new OrderItem(orderId, productId, productName, unitPrice, discount, pictureUrl, units);

        // Act
        orderItem.SetNewDiscount(2.0m);

        // Assert
        Assert.Equal(2.0m, orderItem.Discount);
    }

    [Fact]
    public void SetNewDiscount_ShouldThrowException_WhenDiscountIsNegative()
    {
        // Arrange
        var orderId = new OrderId(Guid.CreateVersion7());
        var productId = Guid.CreateVersion7();
        var productName = "Product1";
        var unitPrice = 10.0m;
        var discount = 1.0m;
        var pictureUrl = new Uri("http://example.com/picture.jpg");
        var units = 2;

        var orderItem = new OrderItem(orderId, productId, productName, unitPrice, discount, pictureUrl, units);

        // Act & Assert
        var exception = Assert.Throws<OrderingDomainException>(() => orderItem.SetNewDiscount(-1.0m));
        Assert.Equal("Discount is not valid", exception.Message);
    }

    [Fact]
    public void AddUnits_ShouldIncreaseUnits()
    {
        // Arrange
        var orderId = new OrderId(Guid.CreateVersion7());
        var productId = Guid.CreateVersion7();
        var productName = "Product1";
        var unitPrice = 10.0m;
        var discount = 1.0m;
        var pictureUrl = new Uri("http://example.com/picture.jpg");
        var units = 2;

        var orderItem = new OrderItem(orderId, productId, productName, unitPrice, discount, pictureUrl, units);

        // Act
        orderItem.AddUnits(3);

        // Assert
        Assert.Equal(5, orderItem.Units);
    }

    [Fact]
    public void AddUnits_ShouldThrowException_WhenUnitsIsNegative()
    {
        // Arrange
        var orderId = new OrderId(Guid.CreateVersion7());
        var productId = Guid.CreateVersion7();
        var productName = "Product1";
        var unitPrice = 10.0m;
        var discount = 1.0m;
        var pictureUrl = new Uri("http://example.com/picture.jpg");
        var units = 2;

        var orderItem = new OrderItem(orderId, productId, productName, unitPrice, discount, pictureUrl, units);

        // Act & Assert
        var exception = Assert.Throws<OrderingDomainException>(() => orderItem.AddUnits(-1));
        Assert.Equal("Invalid units", exception.Message);
    }
}
