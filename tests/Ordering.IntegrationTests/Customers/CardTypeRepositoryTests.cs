using Ordering.Core.CustomerAggregate.Specifications;

namespace Ordering.IntegrationTests.Customers;

public class CardTypeRepositoryTests : BaseIntegrationTest
{
    private readonly CancellationToken _cancellationToken = TestContext.Current.CancellationToken;

    public CardTypeRepositoryTests(OrderingFactory factory) 
        : base(factory)
    {
    }

    [Fact]
    public async Task CreateAsync_ShouldAddCardType()
    {
        // Arrange
        var _cardTypeRepository = Scope.ServiceProvider.GetRequiredService<ICardTypeRepository>();
        var DbContext = Scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
        var cardType = CardType.Create("Visa");

        // Act
        await _cardTypeRepository.CreateAsync(cardType, _cancellationToken);
        await _cardTypeRepository.SaveChangesAsync(_cancellationToken);

        // Assert
        var createdCardType = await DbContext.CardTypes.FirstOrDefaultAsync(c => c.Id == cardType.Id, _cancellationToken);
        Assert.NotNull(createdCardType);
    }

    [Fact]
    public async Task Delete_ShouldRemoveCardType()
    {
        // Arrange
        var _cardTypeRepository = Scope.ServiceProvider.GetRequiredService<ICardTypeRepository>();
        var DbContext = Scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
        var cardType = CardType.Create("Visa2");
        await DbContext.CardTypes.AddAsync(cardType, _cancellationToken);
        await DbContext.SaveChangesAsync(_cancellationToken);

        // Act
        _cardTypeRepository.Delete(cardType);
        await _cardTypeRepository.SaveChangesAsync(_cancellationToken);

        // Assert
        var deletedCardType = await DbContext.CardTypes.Where(c => c.Name == cardType.Name).FirstOrDefaultAsync(_cancellationToken);
        Assert.Null(deletedCardType);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnCardTypes()
    {
        // Arrange
        var _cardTypeRepository = Scope.ServiceProvider.GetRequiredService<ICardTypeRepository>();
        var DbContext = Scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
        var cardType1 = CardType.Create("Visa1");
        var cardType2 = CardType.Create("MasterCard1");

        await DbContext.CardTypes.AddRangeAsync([cardType1, cardType2], _cancellationToken);
        await DbContext.SaveChangesAsync(_cancellationToken);

        // Act
        var cardTypes = await _cardTypeRepository.ListAsync(_cancellationToken);

        // Assert
        Assert.Contains(cardTypes, ct => ct.Name == cardType1.Name);
        Assert.Contains(cardTypes, ct => ct.Name == cardType2.Name);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnCardType()
    {
        // Arrange
        var _cardTypeRepository = Scope.ServiceProvider.GetRequiredService<ICardTypeRepository>();
        var DbContext = Scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
        var cardType = CardType.Create("Visa5");

        await DbContext.CardTypes.AddAsync(cardType, _cancellationToken);
        await DbContext.SaveChangesAsync(_cancellationToken);

        // Act
        var specification = new CardTypeSpecification(cardType.Id);
        var result = await _cardTypeRepository.SingleOrDefaultAsync(specification, _cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cardType.Name, result.Name);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnNull_WhenCardTypeDoesNotExist()
    {
        // Arrange
        var _cardTypeRepository = Scope.ServiceProvider.GetRequiredService<ICardTypeRepository>();
        var DbContext = Scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
        var cardType = CardType.Create("Visa3");

        await DbContext.CardTypes.AddAsync(cardType, _cancellationToken);
        await DbContext.SaveChangesAsync(_cancellationToken);


        // Act
        var specification = new CardTypeSpecification(100);
        var result = await _cardTypeRepository.SingleOrDefaultAsync(specification, _cancellationToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ShouldModifyCardType()
    {
        // Arrange
        var _cardTypeRepository = Scope.ServiceProvider.GetRequiredService<ICardTypeRepository>();
        var DbContext = Scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
        var cardType = CardType.Create("MasterCard3");
        await DbContext.CardTypes.AddAsync(cardType, _cancellationToken);
        await DbContext.SaveChangesAsync(_cancellationToken);

        // Act
        const string newName = "MasterCardUpdated";
        cardType.UpdateName(newName);
        _cardTypeRepository.Update(cardType);
        await _cardTypeRepository.SaveChangesAsync(_cancellationToken);

        // Assert
        var updatedCardType = await DbContext.CardTypes.FirstOrDefaultAsync(c => c.Id == cardType.Id, _cancellationToken);
        Assert.NotNull(updatedCardType);
        Assert.Equal(newName, updatedCardType.Name);
    }
}
