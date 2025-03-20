using Ordering.Core.CustomerAggregate;

namespace Ordering.UseCases.CardTypes
{
    public static class CardTypeExtensions
    {
        public static CardTypeDto ToCardTypeDto(this CardType cardType)
        {
            return new CardTypeDto(cardType.Id, cardType.Name);
        }
    }
}
