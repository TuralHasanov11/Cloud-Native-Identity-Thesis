using Ordering.UseCases.Orders.Queries;

namespace Ordering.IntegrationTests.Orders;

public class ListOrdersByUserQueryTests : IClassFixture<OrderingFactory>
{
    private readonly IMediator _mediator;
    private readonly OrderingDbContext _dbContext;

    public ListOrdersByUserQueryTests(OrderingFactory factory)
    {
        _mediator = factory.Services.GetRequiredService<IMediator>();
        _dbContext = factory.Services.GetRequiredService<OrderingDbContext>();
    }

    [Fact]
    public async Task Handle_ShouldReturnOrders()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var userId = new IdentityId(Guid.CreateVersion7());
        var order1 = new Order(
            userId,
            "John Doe",
            new Address("Street", "City", "State", "Country", "ZipCode"),
            1,
            "1234567890123456",
            "123",
            "John Doe",
            DateTime.UtcNow.AddYears(1));

        var order2 = new Order(
            userId,
            "John Doe",
            new Address("Street", "City", "State", "Country", "ZipCode"),
            1,
            "1234567890123456",
            "123",
            "John Doe",
            DateTime.UtcNow.AddYears(1));

        _dbContext.Orders.Add(order1);
        _dbContext.Orders.Add(order2);
        await _dbContext.SaveChangesAsync();

        var query = new ListOrdersByUserQuery(userId);

        // Act
        var result = await _mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(result.Value, o => o.OrderNumber == order1.Id.Value);
        Assert.Contains(result.Value, o => o.OrderNumber == order2.Id.Value);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoOrdersExist()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var query = new ListOrdersByUserQuery(Guid.CreateVersion7());

        // Act
        var result = await _mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }
}
