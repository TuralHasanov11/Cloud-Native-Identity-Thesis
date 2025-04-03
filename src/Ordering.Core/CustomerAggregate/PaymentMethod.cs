namespace Ordering.Core.CustomerAggregate;

public class PaymentMethod : EntityBase<PaymentMethodId>
{
    public string Alias { get; private set; }

    public string CardNumber { get; private set; }

    public string SecurityNumber { get; private set; }

    public string CardHolderName { get; private set; }

    public DateTime ExpirationDate { get; private set; }

    public int CardTypeId { get; private set; }

    public CardType CardType { get; private set; }

    public PaymentMethod(
        int cardTypeId,
        string alias,
        string cardNumber,
        string securityNumber,
        string cardHolderName,
        DateTime expirationDate)
        : base(new PaymentMethodId(Guid.CreateVersion7()))
    {
        CardNumber = !string.IsNullOrWhiteSpace(cardNumber) ? cardNumber : throw new OrderingDomainException(nameof(cardNumber));
        SecurityNumber = !string.IsNullOrWhiteSpace(securityNumber) ? securityNumber : throw new OrderingDomainException(nameof(securityNumber));
        CardHolderName = !string.IsNullOrWhiteSpace(cardHolderName) ? cardHolderName : throw new OrderingDomainException(nameof(cardHolderName));

        if (expirationDate < DateTime.UtcNow)
        {
            throw new OrderingDomainException(nameof(expirationDate));
        }

        Alias = alias;
        ExpirationDate = expirationDate;
        CardTypeId = cardTypeId;
    }

    public bool IsEqualTo(int cardTypeId, string cardNumber, DateTime expirationDate)
    {
        return CardTypeId == cardTypeId
            && CardNumber == cardNumber
            && ExpirationDate == expirationDate;
    }
}
