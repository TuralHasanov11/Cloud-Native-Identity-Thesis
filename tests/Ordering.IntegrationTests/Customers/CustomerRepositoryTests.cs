using Ordering.Core.CustomerAggregate.Specifications;

namespace Ordering.IntegrationTests.Customers;

public class CustomerRepositoryTests : IClassFixture<OrderingFactory>
{
    private readonly OrderingFactory _factory;

    public CustomerRepositoryTests(OrderingFactory factory)
    {
        _factory = factory;
    }

    [Fact(Skip = "Waiting")]
    public async Task CreateAsync_ShouldAddCustomer()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
        await dbContext.SeedDatabase();

        var repository = new CustomerRepository(dbContext);
        var customer = Customer.Create(new IdentityId(Guid.NewGuid()), "John Doe");

        // Act
        await repository.CreateAsync(customer);
        await repository.SaveChangesAsync();

        // Assert
        var createdCustomer = await dbContext.Customers.FindAsync(customer.Id);
        Assert.NotNull(createdCustomer);
    }

    [Fact(Skip = "Waiting")]
    public async Task Delete_ShouldRemoveCustomer()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
        await dbContext.SeedDatabase();

        var repository = new CustomerRepository(dbContext);
        var customer = Customer.Create(new IdentityId(Guid.NewGuid()), "John Doe");

        await repository.CreateAsync(customer);
        await repository.SaveChangesAsync();

        // Act
        repository.Delete(customer);
        await repository.SaveChangesAsync();

        // Assert
        var deletedCustomer = await dbContext.Customers.FindAsync(customer.Id);
        Assert.Null(deletedCustomer);
    }

    [Fact(Skip = "Waiting")]
    public async Task ListAsync_ShouldReturnCustomers()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
        await dbContext.SeedDatabase();

        var repository = new CustomerRepository(dbContext);

        var customer1 = Customer.Create(new IdentityId(Guid.NewGuid()), "John Doe");
        var customer2 = Customer.Create(new IdentityId(Guid.NewGuid()), "Jane Doe");

        await repository.CreateAsync(customer1);
        await repository.CreateAsync(customer2);
        await repository.SaveChangesAsync();

        var specification = new GetCustomersSpecification();

        // Act
        var customers = await repository.ListAsync(specification);

        // Assert
        Assert.Contains(customers, c => c.Id == customer1.Id);
        Assert.Contains(customers, c => c.Id == customer2.Id);
    }

    [Fact(Skip = "Waiting")]
    public async Task SingleOrDefaultAsync_ShouldReturnCustomer()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
        await dbContext.SeedDatabase();

        var repository = new CustomerRepository(dbContext);
        var customer = Customer.Create(new IdentityId(Guid.NewGuid()), "John Doe");

        await repository.CreateAsync(customer);
        await repository.SaveChangesAsync();
        var specification = new GetCustomerByIdSpecification(customer.Id);

        // Act
        var result = await repository.SingleOrDefaultAsync(specification);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customer.Id, result.Id);
    }

    [Fact(Skip = "Waiting")]
    public async Task SingleOrDefaultAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
        await dbContext.SeedDatabase();

        var repository = new CustomerRepository(dbContext);
        var specification = new GetCustomerByIdSpecification(new CustomerId(Guid.NewGuid()));

        // Act
        var result = await repository.SingleOrDefaultAsync(specification);

        // Assert
        Assert.Null(result);
    }

    [Fact(Skip = "Waiting")]
    public async Task Update_ShouldModifyCustomer()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
        await dbContext.SeedDatabase();

        var repository = new CustomerRepository(dbContext);
        var customer = Customer.Create(new IdentityId(Guid.NewGuid()), "John Doe");

        await repository.CreateAsync(customer);
        await repository.SaveChangesAsync();

        // Act
        customer = Customer.Create(new IdentityId(Guid.NewGuid()), "Jane Doe");
        repository.Update(customer);
        await repository.SaveChangesAsync();

        // Assert
        var updatedCustomer = await dbContext.Customers.FindAsync(customer.Id);
        Assert.NotNull(updatedCustomer);
        Assert.Equal("Jane Doe", updatedCustomer.Name);
    }
}
