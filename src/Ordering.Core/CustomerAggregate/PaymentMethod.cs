namespace Ordering.Core.CustomerAggregate;

public class PaymentMethod : EntityBase<PaymentMethodId>
{
    public string Alias { get; private set; }

    public int CardTypeId { get; private set; }

    public CardType CardType { get; private set; }

    public PaymentMethod(
        int cardTypeId,
        string alias)
        : base(new PaymentMethodId(Guid.CreateVersion7()))
    {
        Alias = alias;
        CardTypeId = cardTypeId;
    }

    public bool IsEqualTo(int cardTypeId)
    {
        return CardTypeId == cardTypeId;
    }
}
