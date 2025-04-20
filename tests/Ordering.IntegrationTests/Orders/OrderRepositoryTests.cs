//namespace Ordering.IntegrationTests.Orders;

//public class OrderRepositoryTests : IClassFixture<OrderingFactory>
//{
//    private readonly OrderingFactory _factory;

//    public OrderRepositoryTests(OrderingFactory factory)
//    {
//        _factory = factory;
//    }

//    [Fact(Skip = "Waiting")]
//    public async Task CreateAsync_ShouldAddOrder()
//    {
//        var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
//        await dbContext.SeedDatabase();

//        var repository = new OrderRepository(dbContext);
//        var order = new Order(
//            new IdentityId(IdentityExtensions.GenerateId()),
//            "fakeName",
//            new Address("street", "city", "state", "country", "zipcode"),
//            cardTypeId: 5,
//            cardNumber: "12",
//            cardSecurityNumber: "123",
//            cardHolderName: "name",
//            cardExpiration: DateTime.UtcNow);

//        await repository.CreateAsync(order);
//        await repository.SaveChangesAsync();

//        var createdOrder = await dbContext.Orders.FindAsync(order.Id);
//        Assert.NotNull(createdOrder);
//    }

//    [Fact(Skip = "Waiting")]
//    public async Task Delete_ShouldRemoveOrder()
//    {
//        // Arrange
//        var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
//        await dbContext.SeedDatabase();

//        var repository = new OrderRepository(dbContext);
//        var order = new Order(
//            new IdentityId(IdentityExtensions.GenerateId()),
//            "fakeName",
//            new Address("street", "city", "state", "country", "zipcode"),
//            cardTypeId: 5,
//            cardNumber: "12",
//            cardSecurityNumber: "123",
//            cardHolderName: "name",
//            cardExpiration: DateTime.UtcNow);

//        await repository.CreateAsync(order);
//        await repository.SaveChangesAsync();

//        // Act
//        repository.Delete(order);
//        await repository.SaveChangesAsync();

//        // Assert
//        var deletedOrder = await dbContext.Orders.FindAsync(order.Id);
//        Assert.Null(deletedOrder);
//    }

//    [Fact(Skip = "Waiting")]
//    public async Task ListAsync_ShouldReturnOrders()
//    {
//        // Arrange
//        var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
//        await dbContext.SeedDatabase();

//        var repository = new OrderRepository(dbContext);
//        var identityId = new IdentityId(IdentityExtensions.GenerateId());

//        var order1 = new Order(
//            identityId,
//            "fakeName",
//            new Address("street", "city", "state", "country", "zipcode"),
//            cardTypeId: 5,
//            cardNumber: "12",
//            cardSecurityNumber: "123",
//            cardHolderName: "name",
//            cardExpiration: DateTime.UtcNow);

//        var order2 = new Order(
//            identityId,
//            "fakeName",
//            new Address("street", "city", "state", "country", "zipcode"),
//            cardTypeId: 5,
//            cardNumber: "12",
//            cardSecurityNumber: "123",
//            cardHolderName: "name",
//            cardExpiration: DateTime.UtcNow);

//        await repository.CreateAsync(order1);
//        await repository.CreateAsync(order2);
//        await repository.SaveChangesAsync();

//        var specification = new GetOrdersByCustomerIdSpecification(identityId);

//        // Act
//        var orders = await repository.ListAsync(specification);

//        // Assert
//        Assert.Contains(orders, o => o.Id == order1.Id);
//        Assert.Contains(orders, o => o.Id == order2.Id);
//    }

//    [Fact(Skip = "Waiting")]
//    public async Task SingleOrDefaultAsync_ShouldReturnOrder()
//    {
//        // Arrange
//        var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
//        await dbContext.SeedDatabase();

//        var repository = new OrderRepository(dbContext);
//        var order = new Order(
//            new IdentityId(IdentityExtensions.GenerateId()),
//            "fakeName",
//            new Address("street", "city", "state", "country", "zipcode"),
//            cardTypeId: 5,
//            cardNumber: "12",
//            cardSecurityNumber: "123",
//            cardHolderName: "name",
//            cardExpiration: DateTime.UtcNow);

//        await repository.CreateAsync(order);
//        await repository.SaveChangesAsync();
//        var specification = new GetOrderByIdSpecification(order.Id);

//        // Act
//        var result = await repository.SingleOrDefaultAsync(specification);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(order.Id, result.Id);
//    }

//    [Fact(Skip = "Waiting")]
//    public async Task SingleOrDefaultAsync_ShouldReturnNull_WhenOrderDoesNotExist()
//    {
//        // Arrange
//        var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
//        await dbContext.SeedDatabase();

//        var repository = new OrderRepository(dbContext);
//        var specification = new GetOrderByIdSpecification(new OrderId(Guid.CreateVersion7()));

//        // Act
//        var result = await repository.SingleOrDefaultAsync(specification);

//        // Assert
//        Assert.Null(result);
//    }

//    [Fact(Skip = "Waiting")]
//    public async Task Update_ShouldModifyOrder()
//    {
//        // Arrange
//        var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
//        await dbContext.SeedDatabase();

//        var repository = new OrderRepository(dbContext);
//        var order = new Order(
//            new IdentityId(IdentityExtensions.GenerateId()),
//            "fakeName",
//            new Address("street", "city", "state", "country", "zipcode"),
//            cardTypeId: 5,
//            cardNumber: "12",
//            cardSecurityNumber: "123",
//            cardHolderName: "name",
//            cardExpiration: DateTime.UtcNow);

//        await repository.CreateAsync(order);
//        await repository.SaveChangesAsync();

//        // Act
//        order.SetAwaitingValidationStatus();
//        repository.Update(order);
//        await repository.SaveChangesAsync();

//        // Assert
//        var updatedOrder = await dbContext.Orders.FindAsync(order.Id);
//        Assert.NotNull(updatedOrder);
//        Assert.Equal(OrderStatus.Submitted, updatedOrder.OrderStatus);
//    }
//}
