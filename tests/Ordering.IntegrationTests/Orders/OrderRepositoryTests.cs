namespace Ordering.IntegrationTests.Orders;

public class OrderRepositoryTests : IClassFixture<OrderingFactory>
{
    private readonly OrderingFactory _factory;

    public OrderRepositoryTests(OrderingFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateAsync_ShouldAddOrder()
    {
        var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
        await dbContext.SeedDatabase();

        var repository = new OrderRepository(dbContext);
        var order = new Order(
            Guid.CreateVersion7(),
            "fakeName",
            new Address("street", "city", "state", "country", "zipcode"),
            cardTypeId: 5,
            cardNumber: "12",
            cardSecurityNumber: "123",
            cardHolderName: "name",
            cardExpiration: DateTime.UtcNow);

        await repository.CreateAsync(order);
        await repository.SaveChangedAsync();

        var createdOrder = await dbContext.Orders.FindAsync(order.Id);
        Assert.NotNull(createdOrder);
    }

    [Fact]
    public async Task Delete_ShouldRemoveOrder()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
        await dbContext.SeedDatabase();

        var repository = new OrderRepository(dbContext);
        var order = new Order(
            Guid.CreateVersion7(),
            "fakeName",
            new Address("street", "city", "state", "country", "zipcode"),
            cardTypeId: 5,
            cardNumber: "12",
            cardSecurityNumber: "123",
            cardHolderName: "name",
            cardExpiration: DateTime.UtcNow);

        await repository.CreateAsync(order);
        await repository.SaveChangedAsync();

        // Act
        repository.Delete(order);
        await repository.SaveChangedAsync();

        // Assert
        var deletedOrder = await dbContext.Orders.FindAsync(order.Id);
        Assert.Null(deletedOrder);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnOrders()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
        await dbContext.SeedDatabase();

        var repository = new OrderRepository(dbContext);

        var order1 = new Order(
            Guid.CreateVersion7(),
            "fakeName",
            new Address("street", "city", "state", "country", "zipcode"),
            cardTypeId: 5,
            cardNumber: "12",
            cardSecurityNumber: "123",
            cardHolderName: "name",
            cardExpiration: DateTime.UtcNow);

        var order2 = new Order(
            Guid.CreateVersion7(),
            "fakeName",
            new Address("street", "city", "state", "country", "zipcode"),
            cardTypeId: 5,
            cardNumber: "12",
            cardSecurityNumber: "123",
            cardHolderName: "name",
            cardExpiration: DateTime.UtcNow);

        await repository.CreateAsync(order1);
        await repository.CreateAsync(order2);
        await repository.SaveChangedAsync();

        var specification = new GetOrdersByCustomerIdSpecification(order1.CustomerId);

        // Act
        var orders = await repository.ListAsync(specification);

        // Assert
        Assert.Contains(orders, o => o.Id == order1.Id);
        Assert.Contains(orders, o => o.Id == order2.Id);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnOrder()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
        await dbContext.SeedDatabase();

        var repository = new OrderRepository(dbContext);
        var order = new Order(
            Guid.CreateVersion7(),
            "fakeName",
            new Address("street", "city", "state", "country", "zipcode"),
            cardTypeId: 5,
            cardNumber: "12",
            cardSecurityNumber: "123",
            cardHolderName: "name",
            cardExpiration: DateTime.UtcNow);

        await repository.CreateAsync(order);
        await repository.SaveChangedAsync();
        var specification = new GetOrderByIdSpecification(order.Id);

        // Act
        var result = await repository.SingleOrDefaultAsync(specification);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(order.Id, result.Id);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnNull_WhenOrderDoesNotExist()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
        await dbContext.SeedDatabase();

        var repository = new OrderRepository(dbContext);
        var specification = new GetOrderByIdSpecification(new OrderId(Guid.CreateVersion7()));

        // Act
        var result = await repository.SingleOrDefaultAsync(specification);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ShouldModifyOrder()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
        await dbContext.SeedDatabase();

        var repository = new OrderRepository(dbContext);
        var order = new Order(
            Guid.CreateVersion7(),
            "fakeName",
            new Address("street", "city", "state", "country", "zipcode"),
            cardTypeId: 5,
            cardNumber: "12",
            cardSecurityNumber: "123",
            cardHolderName: "name",
            cardExpiration: DateTime.UtcNow);

        await repository.CreateAsync(order);
        await repository.SaveChangedAsync();

        // Act
        order.SetAwaitingValidationStatus();
        repository.Update(order);
        await repository.SaveChangedAsync();

        // Assert
        var updatedOrder = await dbContext.Orders.FindAsync(order.Id);
        Assert.NotNull(updatedOrder);
        Assert.Equal(OrderStatus.Submitted, updatedOrder.OrderStatus);
    }
}
