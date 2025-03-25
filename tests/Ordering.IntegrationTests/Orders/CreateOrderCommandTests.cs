using Ordering.UseCases.Orders;
using Ordering.UseCases.Orders.Commands;

namespace Ordering.IntegrationTests.Orders;

public class CreateOrderCommandTests : IClassFixture<OrderingFactory>
{
    private readonly IMediator _mediator;
    private readonly OrderingDbContext _dbContext;

    public CreateOrderCommandTests(OrderingFactory factory)
    {
        _mediator = factory.Services.GetRequiredService<IMediator>();
        _dbContext = factory.Services.GetRequiredService<OrderingDbContext>();
    }

    [Fact]
    public async Task Handle_ShouldCreateOrder()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var orderItems = new List<OrderItemDto>
        {
            new(Guid.NewGuid(), "Product1", new Uri("http://example.com/picture.jpg"), 10.0m, 2, 1.0m),
            new(Guid.NewGuid(), "Product2", new Uri("http://example.com/picture.jpg"), 20.0m, 1, 2.0m)
        };

        var command = new CreateOrderCommand(
            orderItems,
            Guid.NewGuid(),
            "John Doe",
            "City",
            "Street",
            "State",
            "Country",
            "ZipCode",
            "1234567890123456",
            "John Doe",
            DateTime.UtcNow.AddYears(1),
            "123",
            1);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);
        var createdOrder = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Customer.IdentityId == command.UserId);
        var paymentMethod = await _dbContext.PaymentMethods.FirstOrDefaultAsync(p => p.CardNumber == command.CardNumber);
        Assert.NotNull(createdOrder);
        Assert.Equal(command.UserName, createdOrder.Customer.Name);
        Assert.Equal(command.City, createdOrder.Address.City);
        Assert.Equal(command.Street, createdOrder.Address.Street);
        Assert.Equal(command.State, createdOrder.Address.State);
        Assert.Equal(command.Country, createdOrder.Address.Country);
        Assert.Equal(command.ZipCode, createdOrder.Address.ZipCode);
        Assert.Equal(command.CardNumber, paymentMethod?.CardNumber);
        Assert.Equal(command.CardHolderName, paymentMethod?.CardHolderName);
        Assert.Equal(command.CardExpiration, paymentMethod?.ExpirationDate);
        Assert.Equal(command.CardSecurityNumber, paymentMethod?.SecurityNumber);
        Assert.Equal(command.CardTypeId, paymentMethod?.CardTypeId);
        Assert.Equal(orderItems.Count, createdOrder.OrderItems.Count);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenOrderItemsAreEmpty()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var command = new CreateOrderCommand(
            [],
            Guid.NewGuid(),
            "John Doe",
            "City",
            "Street",
            "State",
            "Country",
            "ZipCode",
            "1234567890123456",
            "John Doe",
            DateTime.UtcNow.AddYears(1),
            "123",
            1);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        Assert.False(result.IsSuccess);
    }
}
