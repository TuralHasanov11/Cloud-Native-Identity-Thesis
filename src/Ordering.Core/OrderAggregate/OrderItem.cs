namespace Ordering.Core.OrderAggregate;

public class OrderItem : EntityBase<OrderItemId>
{
    public string ProductName { get; private set; }

    public Uri PictureUrl { get; private set; }

    public decimal UnitPrice { get; private set; }

    public decimal Discount { get; private set; }

    public int Units { get; private set; }

    public Guid ProductId { get; private set; }

    public OrderId OrderId { get; }

    public OrderItem(
        OrderId orderId,
        Guid productId,
        string productName,
        decimal unitPrice,
        decimal discount,
        Uri pictureUrl,
        int units = 1)
        : base(new OrderItemId(Guid.CreateVersion7()))
    {
        if (units <= 0)
        {
            throw new OrderingDomainException("Invalid number of units");
        }

        if (unitPrice * units < discount)
        {
            throw new OrderingDomainException("The total of order item is lower than applied discount");
        }

        OrderId = orderId;
        ProductId = productId;

        ProductName = productName;
        UnitPrice = unitPrice;
        Discount = discount;
        Units = units;
        PictureUrl = pictureUrl;
    }

    public void SetNewDiscount(decimal discount)
    {
        if (discount < 0)
        {
            throw new OrderingDomainException("Discount is not valid");
        }

        Discount = discount;
    }

    public void AddUnits(int units)
    {
        if (units < 0)
        {
            throw new OrderingDomainException("Invalid units");
        }

        Units += units;
    }
}
