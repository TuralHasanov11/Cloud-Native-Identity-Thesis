using Ordering.Core.CustomerAggregate.Specifications;

namespace Ordering.IntegrationTests.Customers;

public class CardTypeRepositoryTests : BaseIntegrationTest
{
    private readonly ICardTypeRepository _cardTypeRepository;

    public CardTypeRepositoryTests(OrderingFactory factory) : base(factory)
    {
        _cardTypeRepository = factory.Services.GetRequiredService<ICardTypeRepository>();
    }

    [Fact]
    public async Task CreateAsync_ShouldAddCardType()
    {
        // Arrange
        var cardType = CardType.Create("Visa");

        // Act
        await _cardTypeRepository.CreateAsync(cardType);
        await _cardTypeRepository.SaveChangesAsync();

        // Assert
        var createdCardType = await DbContext.CardTypes.FindAsync(cardType.Id);
        Assert.NotNull(createdCardType);
    }

    [Fact]
    public async Task Delete_ShouldRemoveCardType()
    {
        // Arrange
        var cardType = CardType.Create("Visa2");
        await DbContext.CardTypes.AddAsync(cardType);
        await DbContext.SaveChangesAsync();

        // Act
        _cardTypeRepository.Delete(cardType);
        await _cardTypeRepository.SaveChangesAsync();

        // Assert
        var deletedCardType = await DbContext.CardTypes.Where(c => c.Name == cardType.Name).FirstOrDefaultAsync();
        Assert.Null(deletedCardType);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnCardTypes()
    {
        // Arrange
        var cardType1 = CardType.Create("Visa1");
        var cardType2 = CardType.Create("MasterCard1");

        await _cardTypeRepository.CreateAsync(cardType1);
        await _cardTypeRepository.CreateAsync(cardType2);
        await _cardTypeRepository.SaveChangesAsync();

        // Act
        var cardTypes = await _cardTypeRepository.ListAsync();

        // Assert
        Assert.Contains(cardTypes, ct => ct.Name == "Visa1");
        Assert.Contains(cardTypes, ct => ct.Name == "MasterCard1");
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnCardType()
    {
        // Arrange
        var cardType = CardType.Create("Visa5");

        await _cardTypeRepository.CreateAsync(cardType);
        await _cardTypeRepository.SaveChangesAsync();
        var specification = new GetCardTypeSpecification(cardType.Id);

        // Act
        var result = await _cardTypeRepository.SingleOrDefaultAsync(specification);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Visa5", result.Name);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnNull_WhenCardTypeDoesNotExist()
    {
        // Arrange
        var cardType = CardType.Create("Visa3");
        await _cardTypeRepository.CreateAsync(cardType);
        await _cardTypeRepository.SaveChangesAsync();

        var specification = new GetCardTypeSpecification(100);

        // Act
        var result = await _cardTypeRepository.SingleOrDefaultAsync(specification);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ShouldModifyCardType()
    {
        // Arrange
        var cardType = CardType.Create("MasterCard3");
        await _cardTypeRepository.CreateAsync(cardType);
        await _cardTypeRepository.SaveChangesAsync();

        // Act
        _cardTypeRepository.Update(cardType);
        await _cardTypeRepository.SaveChangesAsync();

        // Assert
        var updatedCardType = await DbContext.CardTypes.FindAsync(cardType.Id);
        Assert.NotNull(updatedCardType);
        Assert.Equal("MasterCard3", updatedCardType.Name);
    }
}
