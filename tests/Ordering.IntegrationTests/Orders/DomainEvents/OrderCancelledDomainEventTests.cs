namespace Ordering.IntegrationTests.Orders.DomainEvents;

public class OrderCanceledDomainEventTests : IClassFixture<OrderingFactory>
{
    //private readonly IMediator _mediator;
    //private readonly OrderingDbContext _dbContext;
    //private readonly IOrderingIntegrationEventService _orderingIntegrationEventService;
    //private readonly ILogger<OrderCanceledDomainEventHandler> _logger;

    //public OrderCanceledDomainEventTests(OrderingFactory factory)
    //{
    //    _mediator = factory.Services.GetRequiredService<IMediator>();
    //    _dbContext = factory.Services.GetRequiredService<OrderingDbContext>();
    //    _orderingIntegrationEventService = factory.Services.GetRequiredService<IOrderingIntegrationEventService>();
    //    _logger = factory.Services.GetRequiredService<ILogger<OrderCanceledDomainEventHandler>>();
    //}

    //[Fact]
    //public async Task Handle_ShouldPublishOrderStatusChangedToCanceledIntegrationEvent()
    //{
    //    // Arrange
    //    await _dbContext.SeedDatabase();

    //    var customer = Customer.Create(new IdentityId(Guid.CreateVersion7()), "John Doe");
    //    _dbContext.Customers.Add(customer);
    //    await _dbContext.SaveChangesAsync();

    //    var order = new Order(
    //        customer.IdentityId,
    //        "John Doe",
    //        new Address("Street", "City", "State", "Country", "ZipCode"),
    //        1,
    //        "1234567890123456",
    //        "123",
    //        "John Doe",
    //        DateTime.UtcNow.AddYears(1),
    //        customer.Id);

    //    _dbContext.Orders.Add(order);
    //    await _dbContext.SaveChangesAsync();


    //    // Act
    //    await _mediator.Send(new OrderCanceledDomainEvent(order, DateTime.UtcNow));

    //    // Assert
    //    // Verify that the integration event was published (this would require checking the state of the _orderingIntegrationEventService)
    //}

    //[Fact]
    //public async Task Handle_ShouldNotPublishEvent_WhenOrderDoesNotExist()
    //{
    //    // Arrange
    //    await _dbContext.SeedDatabase();

    //    var order = new Order(
    //        new IdentityId(Guid.CreateVersion7()),
    //        "John Doe",
    //        new Address("Street", "City", "State", "Country", "ZipCode"),
    //        1,
    //        "1234567890123456",
    //        "123",
    //        "John Doe",
    //        DateTime.UtcNow.AddYears(1),
    //        new CustomerId(Guid.CreateVersion7()));

    //    var domainEvent = new OrderCanceledDomainEvent(order, DateTime.UtcNow);
    //    var handler = new OrderCanceledDomainEventHandler(
    //        new OrderRepository(_dbContext),
    //        _logger,
    //        new CustomerRepository(_dbContext),
    //        _orderingIntegrationEventService);

    //    // Act
    //    await handler.Handle(domainEvent, default);

    //    // Assert


    //}

    //[Fact]
    //public async Task Handle_ShouldNotPublishEvent_WhenCustomerDoesNotExist()
    //{
    //    // Arrange
    //    await _dbContext.SeedDatabase();

    //    var order = new Order(
    //        new IdentityId(Guid.CreateVersion7()),
    //        "John Doe",
    //        new Address("Street", "City", "State", "Country", "ZipCode"),
    //        1,
    //        "1234567890123456",
    //        "123",
    //        "John Doe",
    //        DateTime.UtcNow.AddYears(1));

    //    _dbContext.Orders.Add(order);
    //    await _dbContext.SaveChangesAsync();

    //    var domainEvent = new OrderCanceledDomainEvent(order, DateTime.UtcNow);
    //    var handler = new OrderCanceledDomainEventHandler(
    //        new OrderRepository(_dbContext),
    //        _logger,
    //        new CustomerRepository(_dbContext),
    //        _orderingIntegrationEventService);

    //    // Act
    //    await handler.Handle(domainEvent, default);

    //    // Assert
    //    // Verify that the integration event was not published (this would require checking the state of the _orderingIntegrationEventService)
    //}
}
