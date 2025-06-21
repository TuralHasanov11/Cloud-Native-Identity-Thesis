using Ordering.Core.OrderAggregate.Specifications;

namespace Ordering.IntegrationTests.Orders;

public class OrderRepositoryTests : BaseIntegrationTest
{
    private readonly IOrderRepository _repository;
    private readonly CancellationToken _cancellationToken = TestContext.Current.CancellationToken;

    public OrderRepositoryTests(OrderingFactory factory) : base(factory)
    {
        _repository = factory.Services.GetRequiredService<IOrderRepository>();
    }

    [Fact]
    public async Task CreateAsync_ShouldAddOrder()
    {
        var cardType = CardType.Create("Visa");
        DbContext.CardTypes.Add(cardType);
        await DbContext.SaveChangesAsync(_cancellationToken);

        var order = new Order(
            new IdentityId(IdentityExtensions.GenerateId()),
            "fakeName",
            new Address("street", "city", "state", "country", "zipcode"),
            cardType.Id);

        await _repository.CreateAsync(order, _cancellationToken);
        await _repository.SaveChangesAsync(_cancellationToken);

        var createdOrder = await DbContext.Orders.FirstOrDefaultAsync(o => o.Id == order.Id, cancellationToken: _cancellationToken);

        Assert.NotNull(createdOrder);
        Assert.Equal(order.Id, createdOrder.Id);
    }

    [Fact]
    public async Task Delete_ShouldRemoveOrder()
    {
        // Arrange
        var cardType = CardType.Create("Visa");
        DbContext.CardTypes.Add(cardType);
        await DbContext.SaveChangesAsync(_cancellationToken);

        var order = new Order(
            new IdentityId(IdentityExtensions.GenerateId()),
            "fakeName",
            new Address("street", "city", "state", "country", "zipcode"),
            cardType.Id);

        await DbContext.Orders.AddAsync(order, _cancellationToken);
        await DbContext.SaveChangesAsync(_cancellationToken);

        // Act
        _repository.Delete(order);
        await _repository.SaveChangesAsync(_cancellationToken);

        // Assert
        var deletedOrder = await DbContext.Orders.FirstOrDefaultAsync(o => o.Id == order.Id, cancellationToken: _cancellationToken);
        Assert.Null(deletedOrder);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnOrders()
    {
        // Arrange
        var cardType = CardType.Create("Visa");
        DbContext.CardTypes.Add(cardType);

        var identityId = new IdentityId(IdentityExtensions.GenerateId());
        var customer = Customer.Create(identityId, "fakeName");

        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync(_cancellationToken);

        var order1 = new Order(
            identityId,
            "fakeName",
            new Address("street", "city", "state", "country", "zipcode"),
            cardTypeId: cardType.Id,
            customer.Id);

        var order2 = new Order(
            identityId,
            "fakeName",
            new Address("street", "city", "state", "country", "zipcode"),
            cardTypeId: cardType.Id,
            customer.Id);

        DbContext.Orders.AddRange([order1, order2]);
        await DbContext.SaveChangesAsync(_cancellationToken);

        var specification = new GetOrdersByCustomerIdSpecification(identityId);

        // Act
        var orders = await _repository.ListAsync(specification, _cancellationToken);

        // Assert
        Assert.Contains(orders, o => o.Id == order1.Id);
        Assert.Contains(orders, o => o.Id == order2.Id);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnOrder()
    {
        // Arrange
        var cardType = CardType.Create("Visa");
        DbContext.CardTypes.Add(cardType);
        await DbContext.SaveChangesAsync(_cancellationToken);

        var order = new Order(
            new IdentityId(IdentityExtensions.GenerateId()),
            "fakeName",
            new Address("street", "city", "state", "country", "zipcode"),
            cardType.Id);

        await _repository.CreateAsync(order, _cancellationToken);
        await _repository.SaveChangesAsync(_cancellationToken);
        var specification = new GetOrderByIdSpecification(order.Id);

        // Act
        var result = await _repository.SingleOrDefaultAsync(specification, _cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(order.Id, result.Id);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnNull_WhenOrderDoesNotExist()
    {
        // Arrange
        var specification = new GetOrderByIdSpecification(new OrderId(Guid.CreateVersion7()));

        // Act
        var result = await _repository.SingleOrDefaultAsync(specification, _cancellationToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ShouldModifyOrder()
    {
        var cardType = CardType.Create("Visa");
        DbContext.CardTypes.Add(cardType);
        await DbContext.SaveChangesAsync(_cancellationToken);

        // Arrange
        var order = new Order(
            new IdentityId(IdentityExtensions.GenerateId()),
            "fakeName",
            new Address("street", "city", "state", "country", "zipcode"),
            cardType.Id);

        await _repository.CreateAsync(order, _cancellationToken);
        await _repository.SaveChangesAsync(_cancellationToken);

        // Act
        order.SetAwaitingValidationStatus();
        _repository.Update(order);
        await _repository.SaveChangesAsync(_cancellationToken);

        // Assert
        var updatedOrder = await _repository.SingleOrDefaultAsync(
            new GetOrderByIdSpecification(order.Id),
            _cancellationToken);
        Assert.NotNull(updatedOrder);
        Assert.Equal(OrderStatus.AwaitingValidation, updatedOrder.OrderStatus);
    }
}
