using Ordering.Core.Exceptions;
using Ordering.Core.OrderAggregate.Events;

namespace Ordering.UnitTests;

public class OrderAggregateTests
{
    [Fact]
    public void CreateOrderItem_Succeeds()
    {
        var orderId = new OrderId(Guid.CreateVersion7());
        var productId = Guid.CreateVersion7();
        const string productName = "FakeProductName";
        const int unitPrice = 12;
        const int discount = 15;
        var pictureUrl = new Uri("https://nuxt.com");
        const int units = 5;

        var fakeOrderItem = new OrderItem(orderId, productId, productName, unitPrice, discount, pictureUrl, units);

        Assert.NotNull(fakeOrderItem);
    }

    [Fact]
    public void CreateOrderItem_With_InvalidNumberOfUnits_Fails()
    {
        var orderId = new OrderId(Guid.CreateVersion7());
        var productId = Guid.CreateVersion7();
        const string productName = "FakeProductName";
        const int unitPrice = 12;
        const int discount = 15;
        var pictureUrl = new Uri("https://nuxt.com");
        const int units = -5;

        Assert.Throws<OrderingDomainException>(
            () => new OrderItem(orderId, productId, productName, unitPrice, discount, pictureUrl, units));
    }

    [Fact]
    public void CreateOrderItem_With_InvalidTotalOfOrderItem_LowerThan_Discount_Fails()
    {
        var orderId = new OrderId(Guid.CreateVersion7());
        var productId = Guid.CreateVersion7();
        const string productName = "FakeProductName";
        const int unitPrice = 12;
        const int discount = 15;
        var pictureUrl = new Uri("https://nuxt.com");
        const int units = 1;

        Assert.Throws<OrderingDomainException>(
            () => new OrderItem(orderId, productId, productName, unitPrice, discount, pictureUrl, units));
    }

    [Fact]
    public void Setting_InvalidDiscount_Fails()
    {
        var orderId = new OrderId(Guid.CreateVersion7());
        var productId = Guid.CreateVersion7();
        const string productName = "FakeProductName";
        const int unitPrice = 12;
        const int discount = 15;
        var pictureUrl = new Uri("https://nuxt.com");
        const int units = 5;

        var fakeOrderItem = new OrderItem(orderId, productId, productName, unitPrice, discount, pictureUrl, units);

        Assert.Throws<OrderingDomainException>(() => fakeOrderItem.SetNewDiscount(-1));
    }

    [Fact]
    public void Setting_InvalidUnits_Fails()
    {
        var orderId = new OrderId(Guid.CreateVersion7());
        var productId = Guid.CreateVersion7();
        const string productName = "FakeProductName";
        const int unitPrice = 12;
        const int discount = 15;
        var pictureUrl = new Uri("https://nuxt.com");
        const int units = 5;

        var fakeOrderItem = new OrderItem(orderId, productId, productName, unitPrice, discount, pictureUrl, units);

        Assert.Throws<OrderingDomainException>(() => fakeOrderItem.AddUnits(-1));
    }

    [Fact]
    public void Adding_TwoTimes_OnTheSameItem_Then_TheTotalOfOrder_ShouldBe_TheSumOfTwoItems()
    {
        var address = new AddressBuilder().Build();

        var productId = Guid.CreateVersion7();

        var order = new OrderBuilder(address)
            .AddOne(productId, "cup", 10.0m, 0, new Uri(string.Empty))
            .AddOne(productId, "cup", 10.0m, 0, new Uri(string.Empty))
            .Build();

        Assert.Equal(20.0m, order.GetTotal());
    }

    [Fact]
    public void Add_NewOrder_Raises_NewDomainEvent()
    {
        const string street = "fakeStreet";
        const string city = "FakeCity";
        const string state = "fakeState";
        const string country = "fakeCountry";
        const string zipcode = "FakeZipCode";
        const int cardTypeId = 5;
        const string cardNumber = "12";
        const string cardSecurityNumber = "123";
        const string cardHolderName = "FakeName";
        DateTime cardExpiration = DateTime.UtcNow.AddYears(1);

        var fakeOrder = new Order(
            Guid.CreateVersion7(),
            "fakeName",
            new Address(street, city, state, country, zipcode),
            cardTypeId,
            cardNumber,
            cardSecurityNumber,
            cardHolderName,
            cardExpiration);

        Assert.Single(fakeOrder.DomainEvents);
        Assert.IsType<OrderStartedDomainEvent>(fakeOrder.DomainEvents.First());
    }
}
