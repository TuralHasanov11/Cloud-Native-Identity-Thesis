using Ordering.Core.CustomerAggregate.Specifications;

namespace Ordering.IntegrationTests.Customers;

public class CustomerRepositoryTests : BaseIntegrationTest
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerRepositoryTests(OrderingFactory factory) : base(factory)
    {
        _customerRepository = factory.Services.GetRequiredService<ICustomerRepository>();
    }

    [Fact]
    public async Task CreateAsync_ShouldAddCustomer()
    {
        // Arrange
        var customer = Customer.Create(new IdentityId(IdentityExtensions.GenerateId()), "John Doe");

        // Act
        await _customerRepository.CreateAsync(customer);
        await _customerRepository.SaveChangesAsync();

        // Assert
        var createdCustomer = await DbContext.Customers.FindAsync(customer.Id);
        Assert.NotNull(createdCustomer);
    }

    [Fact]
    public async Task Delete_ShouldRemoveCustomer()
    {
        // Arrange
        var customer = Customer.Create(new IdentityId(IdentityExtensions.GenerateId()), "John Doe");

        await _customerRepository.CreateAsync(customer);
        await _customerRepository.SaveChangesAsync();

        // Act
        _customerRepository.Delete(customer);
        await _customerRepository.SaveChangesAsync();

        // Assert
        var deletedCustomer = await DbContext.Customers.FindAsync(customer.Id);
        Assert.Null(deletedCustomer);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnCustomers()
    {
        // Arrange
        var customer1 = Customer.Create(new IdentityId(IdentityExtensions.GenerateId()), "John Doe");
        var customer2 = Customer.Create(new IdentityId(IdentityExtensions.GenerateId()), "Jane Doe");

        await _customerRepository.CreateAsync(customer1);
        await _customerRepository.CreateAsync(customer2);
        await _customerRepository.SaveChangesAsync();

        var specification = new GetCustomersSpecification();

        // Act
        var customers = await _customerRepository.ListAsync(specification);

        // Assert
        Assert.Contains(customers, c => c.Id == customer1.Id);
        Assert.Contains(customers, c => c.Id == customer2.Id);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnCustomer()
    {
        // Arrange
        var customer = Customer.Create(new IdentityId(IdentityExtensions.GenerateId()), "John Doe");

        await _customerRepository.CreateAsync(customer);
        await _customerRepository.SaveChangesAsync();
        var specification = new GetCustomerByIdSpecification(customer.Id);

        // Act
        var result = await _customerRepository.SingleOrDefaultAsync(specification);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customer.Id, result.Id);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
    {
        // Arrange
        var specification = new GetCustomerByIdSpecification(new CustomerId(Guid.CreateVersion7()));

        // Act
        var result = await _customerRepository.SingleOrDefaultAsync(specification);

        // Assert
        Assert.Null(result);
    }

    [Fact(Skip = "Not Ready")]
    public async Task Update_ShouldModifyCustomer()
    {
        // Arrange
        var customer = Customer.Create(new IdentityId(IdentityExtensions.GenerateId()), "Jane Doe");
        await _customerRepository.CreateAsync(customer);
        await _customerRepository.SaveChangesAsync();

        // Act
        _customerRepository.Update(customer);
        await _customerRepository.SaveChangesAsync();

        // Assert
        var updatedCustomer = await DbContext.Customers.FindAsync(customer.Id);
        Assert.NotNull(updatedCustomer);
        Assert.Equal("Jane Doe", updatedCustomer.Name);
    }
}
