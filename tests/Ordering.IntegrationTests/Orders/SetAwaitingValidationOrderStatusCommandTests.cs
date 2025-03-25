using Ordering.UseCases.Orders.Commands;

namespace Ordering.IntegrationTests.Orders;

public class SetAwaitingValidationOrderStatusCommandTests : IClassFixture<OrderingFactory>
{
    private readonly IMediator _mediator;
    private readonly OrderingDbContext _dbContext;

    public SetAwaitingValidationOrderStatusCommandTests(OrderingFactory factory)
    {
        _mediator = factory.Services.GetRequiredService<IMediator>();
        _dbContext = factory.Services.GetRequiredService<OrderingDbContext>();
    }

    [Fact]
    public async Task Handle_ShouldSetOrderStatusToAwaitingValidation()
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

        var command = new SetAwaitingValidationOrderStatusCommand(order.Id.Value);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);
        var updatedOrder = await _dbContext.Orders.FindAsync(order.Id);
        Assert.Equal(OrderStatus.AwaitingValidation, updatedOrder.OrderStatus);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var command = new SetAwaitingValidationOrderStatusCommand(Guid.CreateVersion7());

        // Act
        var result = await _mediator.Send(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.NotFound, result.Status);
    }
}
