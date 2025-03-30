namespace Ordering.IntegrationTests.Customers;

public class CardTypeRepositoryTests : IClassFixture<OrderingFactory>
{
    private readonly OrderingFactory _factory;

    public CardTypeRepositoryTests(OrderingFactory factory)
    {
        _factory = factory;
    }

    [Fact(Skip = "Waiting")]
    public async Task CreateAsync_ShouldAddCardType()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
        await dbContext.SeedDatabase();

        var repository = new CardTypeRepository(dbContext);
        var cardType = CardType.Create("Visa");

        // Act
        await repository.CreateAsync(cardType);
        await repository.SaveChangesAsync();

        // Assert
        var createdCardType = await dbContext.CardTypes.FindAsync(cardType.Id);
        Assert.NotNull(createdCardType);
    }

    [Fact(Skip = "Waiting")]
    public async Task Delete_ShouldRemoveCardType()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
        await dbContext.SeedDatabase();

        var repository = new CardTypeRepository(dbContext);
        var cardType = CardType.Create("MasterCard");

        await repository.CreateAsync(cardType);
        await repository.SaveChangesAsync();

        // Act
        repository.Delete(cardType);
        await repository.SaveChangesAsync();

        // Assert
        var deletedCardType = await dbContext.CardTypes.FindAsync(cardType.Id);
        Assert.Null(deletedCardType);
    }

    [Fact(Skip = "Waiting")]
    public async Task ListAsync_ShouldReturnCardTypes()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
        await dbContext.SeedDatabase();

        var repository = new CardTypeRepository(dbContext);

        var cardType1 = CardType.Create("Visa");
        var cardType2 = CardType.Create("MasterCard");

        await repository.CreateAsync(cardType1);
        await repository.CreateAsync(cardType2);
        await repository.SaveChangesAsync();

        // Act
        var cardTypes = await repository.ListAsync();

        // Assert
        Assert.Contains(cardTypes, ct => ct.Name == "Visa");
        Assert.Contains(cardTypes, ct => ct.Name == "MasterCard");
    }

    //[Fact(Skip = "Waiting")]
    //public async Task SingleOrDefaultAsync_ShouldReturnCardType()
    //{
    //    // Arrange
    //    var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
    //    await dbContext.SeedDatabase();

    //    var repository = new CardTypeRepository(dbContext);
    //    var cardType = CardType.Create("Visa");

    //    await repository.CreateAsync(cardType);
    //    await repository.SaveChangesAsync();
    //    var specification = new Specification<CardType>(ct => ct.Name == "Visa");

    //    // Act
    //    var result = await repository.SingleOrDefaultAsync(specification);

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.Equal("Visa", result.Name);
    //}

    //[Fact(Skip = "Waiting")]
    //public async Task SingleOrDefaultAsync_ShouldReturnNull_WhenCardTypeDoesNotExist()
    //{
    //    // Arrange
    //    var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
    //    await dbContext.SeedDatabase();

    //    var repository = new CardTypeRepository(dbContext);
    //    var specification = new Specification<CardType>(ct => ct.Name == "NonExistent");

    //    // Act
    //    var result = await repository.SingleOrDefaultAsync(specification);

    //    // Assert
    //    Assert.Null(result);
    //}

    [Fact(Skip = "Waiting")]
    public async Task Update_ShouldModifyCardType()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<OrderingDbContext>();
        await dbContext.SeedDatabase();

        var repository = new CardTypeRepository(dbContext);
        var cardType = CardType.Create("Visa");

        await repository.CreateAsync(cardType);
        await repository.SaveChangesAsync();

        // Act
        cardType = CardType.Create("MasterCard");
        repository.Update(cardType);
        await repository.SaveChangesAsync();

        // Assert
        var updatedCardType = await dbContext.CardTypes.FindAsync(cardType.Id);
        Assert.NotNull(updatedCardType);
        Assert.Equal("MasterCard", updatedCardType.Name);
    }
}
