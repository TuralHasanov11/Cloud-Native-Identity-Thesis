using Ordering.Core.CustomerAggregate.Specifications;

namespace Ordering.IntegrationTests.Customers;

public class CustomerRepositoryTests : BaseIntegrationTest
{
    private readonly CancellationToken _cancellationToken = TestContext.Current.CancellationToken;

    public CustomerRepositoryTests(OrderingFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CreateAsync_ShouldAddCustomer()
    {
        // Arrange
        var _customerRepository = Scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
        var DbContext = Scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
        var customer = Customer.Create(new IdentityId(IdentityExtensions.GenerateId()), "John Doe");

        // Act
        await _customerRepository.CreateAsync(customer, _cancellationToken);
        await _customerRepository.SaveChangesAsync(_cancellationToken);

        // Assert
        var createdCustomer = await DbContext.Customers.FirstOrDefaultAsync(c => c.Id == customer.Id, _cancellationToken);
        Assert.NotNull(createdCustomer);
    }

    [Fact]
    public async Task Delete_ShouldRemoveCustomer()
    {
        // Arrange
        var _customerRepository = Scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
        var DbContext = Scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
        var customer = Customer.Create(new IdentityId(IdentityExtensions.GenerateId()), "John Doe");

        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync(_cancellationToken);

        // Act
        _customerRepository.Delete(customer);
        await _customerRepository.SaveChangesAsync(_cancellationToken);

        // Assert
        var deletedCustomer = await DbContext.Customers.FirstOrDefaultAsync(c => c.Id == customer.Id, _cancellationToken);
        Assert.Null(deletedCustomer);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnCustomers()
    {
        // Arrange
        var _customerRepository = Scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
        var DbContext = Scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
        var customer1 = Customer.Create(new IdentityId(IdentityExtensions.GenerateId()), "John Doe");
        var customer2 = Customer.Create(new IdentityId(IdentityExtensions.GenerateId()), "Jane Doe");

        DbContext.Customers.AddRange(customer1, customer2);
        await DbContext.SaveChangesAsync(_cancellationToken);

        var specification = new CustomerSpecification();

        // Act
        var customers = await _customerRepository.ListAsync(specification, _cancellationToken);

        // Assert
        Assert.Contains(customers, c => c.Id == customer1.Id);
        Assert.Contains(customers, c => c.Id == customer2.Id);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnCustomer()
    {
        // Arrange
        var _customerRepository = Scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
        var DbContext = Scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
        var customer = Customer.Create(new IdentityId(IdentityExtensions.GenerateId()), "John Doe");

        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync(_cancellationToken);
        var specification = new CustomerSpecification(customer.Id);

        // Act
        var result = await _customerRepository.SingleOrDefaultAsync(specification, _cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customer.Id, result.Id);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
    {
        // Arrange
        var _customerRepository = Scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
        var specification = new CustomerSpecification(new CustomerId(Guid.CreateVersion7()));

        // Act
        var result = await _customerRepository.SingleOrDefaultAsync(specification, _cancellationToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ShouldModifyCustomer()
    {
        // Arrange
        var _customerRepository = Scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
        var DbContext = Scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
        var customer = Customer.Create(new IdentityId(IdentityExtensions.GenerateId()), "Jane Doe");
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync(_cancellationToken);

        // Act
        _customerRepository.Update(customer);
        await _customerRepository.SaveChangesAsync(_cancellationToken);

        // Assert
        var updatedCustomer = await DbContext.Customers.FirstOrDefaultAsync(c => c.Id == customer.Id, _cancellationToken);
        Assert.NotNull(updatedCustomer);
        Assert.Equal(customer.Name, updatedCustomer.Name);
    }
}
