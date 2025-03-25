using Catalog.UseCases.Products.DeleteById;

namespace Catalog.IntegrationTests.Products;

public class DeleteProductByIdCommandTests : IClassFixture<CatalogFactory>
{
    private readonly IMediator _mediator;
    private readonly CatalogDbContext _dbContext;

    public DeleteProductByIdCommandTests(CatalogFactory factory)
    {
        _mediator = factory.Services.GetRequiredService<IMediator>();
        _dbContext = factory.Services.GetRequiredService<CatalogDbContext>();
    }

    [Fact]
    public async Task Handle_ShouldDeleteProduct()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var product = Product.Create(
            "Product1",
            "Description1",
            10.0m,
            new ProductTypeId(Guid.NewGuid()),
            new BrandId(Guid.NewGuid()),
            100,
            10,
            200);

        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        var command = new DeleteProductByIdCommand(product.Id.Value);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);
        var deletedProduct = await _dbContext.Products.FindAsync(product.Id);
        Assert.Null(deletedProduct);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var command = new DeleteProductByIdCommand(Guid.NewGuid());

        // Act
        var result = await _mediator.Send(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsNotFound());
    }

    [Fact]
    public async Task Handle_ShouldThrowTaskCancelledException_WhenCancellationIsRequested()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var product = Product.Create(
            "Product1",
            "Description1",
            10.0m,
            new ProductTypeId(Guid.NewGuid()),
            new BrandId(Guid.NewGuid()),
            100,
            10,
            200);

        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        var command = new DeleteProductByIdCommand(product.Id.Value);

        using var cancellationTokenSource = new CancellationTokenSource();
        await cancellationTokenSource.CancelAsync();

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(async () =>
        {
            await _mediator.Send(command, cancellationTokenSource.Token);
        });
    }
}
