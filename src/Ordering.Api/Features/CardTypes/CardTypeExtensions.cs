namespace Ordering.Api.Features.CardTypes;

public static class CardTypeExtensions
{
    public static CardTypeDto ToCardTypeDto(this CardType cardType)
    {
        return new CardTypeDto(cardType.Id, cardType.Name);
    }
}
