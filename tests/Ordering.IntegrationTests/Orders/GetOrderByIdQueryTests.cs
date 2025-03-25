using Ordering.UseCases.Orders.Queries;

namespace Ordering.IntegrationTests.Orders;

public class GetOrderByIdQueryTests : IClassFixture<OrderingFactory>
{
    private readonly IMediator _mediator;
    private readonly OrderingDbContext _dbContext;

    public GetOrderByIdQueryTests(OrderingFactory factory)
    {
        _mediator = factory.Services.GetRequiredService<IMediator>();
        _dbContext = factory.Services.GetRequiredService<OrderingDbContext>();
    }

    [Fact]
    public async Task Handle_ShouldReturnOrder()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var order = new Order(
            new IdentityId(Guid.CreateVersion7()),
            "John Doe",
            new Address("Street", "City", "State", "Country", "ZipCode"),
            1,
            "1234567890123456",
            "123",
            "John Doe",
            DateTime.UtcNow.AddYears(1));

        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();

        var query = new GetOrderByIdQuery(order.Id.Value);

        // Act
        var result = await _mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(order.Id.Value, result.Value.OrderNumber);
        Assert.Equal(order.OrderStatus.ToString(), result.Value.Status);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var query = new GetOrderByIdQuery(Guid.NewGuid());

        // Act
        var result = await _mediator.Send(query);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.NotFound, result.Status);
    }
}
