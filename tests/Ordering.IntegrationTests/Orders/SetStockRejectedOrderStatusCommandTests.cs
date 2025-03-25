using Ordering.UseCases.Orders.Commands;

namespace Ordering.IntegrationTests.Orders;

public class SetStockRejectedOrderStatusCommandTests : IClassFixture<OrderingFactory>
{
    private readonly IMediator _mediator;
    private readonly OrderingDbContext _dbContext;

    public SetStockRejectedOrderStatusCommandTests(OrderingFactory factory)
    {
        _mediator = factory.Services.GetRequiredService<IMediator>();
        _dbContext = factory.Services.GetRequiredService<OrderingDbContext>();
    }

    [Fact]
    public async Task Handle_ShouldSetOrderStatusToCancelled_WhenStockIsRejected()
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

        var productId = Guid.CreateVersion7();
        order.AddOrderItem(productId, "Product1", 10.0m, 1.0m, new Uri("http://example.com/picture.jpg"), 2);

        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();

        var command = new SetStockRejectedOrderStatusCommand(order.Id.Value, new List<Guid> { productId });

        // Act
        var result = await _mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);
        var updatedOrder = await _dbContext.Orders.FindAsync(order.Id);
        Assert.Equal(OrderStatus.Cancelled, updatedOrder.OrderStatus);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var command = new SetStockRejectedOrderStatusCommand(Guid.CreateVersion7(), new List<Guid> { Guid.CreateVersion7() });

        // Act
        var result = await _mediator.Send(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.NotFound, result.Status);
    }
}
