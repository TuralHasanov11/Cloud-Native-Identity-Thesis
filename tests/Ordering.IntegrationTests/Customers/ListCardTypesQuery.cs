using Ordering.UseCases.CardTypes.Queries;

namespace Ordering.IntegrationTests.Customers;

public class ListCardTypesQueryTests : IClassFixture<OrderingFactory>
{
    private readonly IMediator _mediator;
    private readonly OrderingDbContext _dbContext;

    public ListCardTypesQueryTests(OrderingFactory factory)
    {
        _mediator = factory.Services.GetRequiredService<IMediator>();
        _dbContext = factory.Services.GetRequiredService<OrderingDbContext>();
    }

    [Fact]
    public async Task Handle_ShouldReturnCardTypes()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var query = new ListCardTypesQuery();

        // Act
        var result = await _mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(result.Value.Count(), OrderingDbContextExtensions.GetCardTypes().Length);
    }
}
