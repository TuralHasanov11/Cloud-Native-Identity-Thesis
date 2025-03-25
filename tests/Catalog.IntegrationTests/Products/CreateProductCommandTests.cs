using Catalog.UseCases.Products.Create;

namespace Catalog.IntegrationTests.Products;

public class CreateProductCommandTests : IClassFixture<CatalogFactory>
{
    private readonly IMediator _mediator;
    private readonly CatalogDbContext _dbContext;

    public CreateProductCommandTests(CatalogFactory factory)
    {
        _mediator = factory.Services.GetRequiredService<IMediator>();
        _dbContext = factory.Services.GetRequiredService<CatalogDbContext>();
    }

    [Fact]
    public async Task Handle_ShouldCreateProduct()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var command = new CreateProductCommand(
            Name: "Product1",
            Description: "Description1",
            Price: 10.0m,
            ProductTypeId: Guid.NewGuid(),
            BrandId: Guid.NewGuid(),
            AvailableStock: 100,
            RestockThreshold: 10,
            MaxStockThreshold: 200);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);
        var createdProduct = await _dbContext.Products.FindAsync(result.Value.Id);
        Assert.NotNull(createdProduct);
        Assert.Equal(command.Name, createdProduct.Name);
        Assert.Equal(command.Description, createdProduct.Description);
        Assert.Equal(command.Price, createdProduct.Price);
        Assert.Equal(command.AvailableStock, createdProduct.AvailableStock);
        Assert.Equal(command.RestockThreshold, createdProduct.RestockThreshold);
        Assert.Equal(command.MaxStockThreshold, createdProduct.MaxStockThreshold);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenProductNameIsEmpty()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var command = new CreateProductCommand(
            Name: string.Empty,
            Description: "Description1",
            Price: 10.0m,
            ProductTypeId: Guid.NewGuid(),
            BrandId: Guid.NewGuid(),
            AvailableStock: 100,
            RestockThreshold: 10,
            MaxStockThreshold: 200
        );

        // Act
        var result = await _mediator.Send(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.ValidationErrors);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenProductPriceIsNegative()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var command = new CreateProductCommand(
            Name: "Product1",
            Description: "Description1",
            Price: -10.0m,
            ProductTypeId: Guid.NewGuid(),
            BrandId: Guid.NewGuid(),
            AvailableStock: 100,
            RestockThreshold: 10,
            MaxStockThreshold: 200
        );

        // Act
        var result = await _mediator.Send(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.ValidationErrors);
    }

    [Fact]
    public async Task Handle_ShouldThrowTaskCancelledException_WhenCancellationIsRequested()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var command = new CreateProductCommand(
            Name: "Product1",
            Description: "Description1",
            Price: 10.0m,
            ProductTypeId: Guid.NewGuid(),
            BrandId: Guid.NewGuid(),
            AvailableStock: 100,
            RestockThreshold: 10,
            MaxStockThreshold: 200);

        using var cancellationTokenSource = new CancellationTokenSource();
        await cancellationTokenSource.CancelAsync();

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(async () =>
        {
            await _mediator.Send(command, cancellationTokenSource.Token);
        });
    }
}
