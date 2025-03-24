namespace Ordering.Core.OrderAggregate;

public sealed class Order
    : EntityBase<OrderId>, IAggregateRoot
{
    public DateTime OrderDate { get; private set; }

    public Address Address { get; private set; }

    public CustomerId? CustomerId { get; private set; }

    public Customer Customer { get; }

    public OrderStatus OrderStatus { get; private set; }

    public string Description { get; private set; }

    public bool IsDraft { get; private set; }

    public ICollection<OrderItem> OrderItems { get; }

    public PaymentMethodId? PaymentMethodId { get; private set; }

    public static Order NewDraft()
    {
        return new Order();
    }

    protected Order()
        : base(new OrderId(Guid.CreateVersion7()))
    {
        IsDraft = true;
    }

    public Order(
        Guid userId,
        string userName,
        Address address,
        int cardTypeId,
        string cardNumber,
        string cardSecurityNumber,
        string cardHolderName,
        DateTime cardExpiration,
        CustomerId? customerId = null,
        PaymentMethodId? paymentMethodId = null)
        : base(new OrderId(Guid.CreateVersion7()))
    {
        CustomerId = customerId;
        PaymentMethodId = paymentMethodId;
        OrderStatus = OrderStatus.Submitted;
        OrderDate = DateTime.UtcNow;
        Address = address;

        AddOrderStartedDomainEvent(
            userId,
            userName,
            cardTypeId,
            cardNumber,
            cardSecurityNumber,
            cardHolderName,
            cardExpiration);
    }

    public void AddOrderItem(Guid productId, string productName, decimal unitPrice, decimal discount, Uri pictureUrl, int units = 1)
    {
        var existingOrderForProduct = OrderItems.SingleOrDefault(o => o.ProductId == productId);

        if (existingOrderForProduct != null)
        {
            if (discount > existingOrderForProduct.Discount)
            {
                existingOrderForProduct.SetNewDiscount(discount);
            }

            existingOrderForProduct.AddUnits(units);
        }
        else
        {
            var orderItem = new OrderItem(Id, productId, productName, unitPrice, discount, pictureUrl, units);
            OrderItems.Add(orderItem);
        }
    }

    public void VerifyPayment(CustomerId customerId, PaymentMethodId paymentId)
    {
        CustomerId = customerId;
        PaymentMethodId = paymentId;
    }

    public void SetAwaitingValidationStatus()
    {
        if (OrderStatus == OrderStatus.Submitted)
        {
            AddDomainEvent(new OrderStatusChangedToAwaitingValidationDomainEvent(Id, OrderItems, DateTime.UtcNow));
            OrderStatus = OrderStatus.AwaitingValidation;
        }
    }

    public void ConfirmStock()
    {
        if (OrderStatus == OrderStatus.AwaitingValidation)
        {
            AddDomainEvent(new OrderStatusChangedToStockConfirmedDomainEvent(Id, DateTime.UtcNow));

            OrderStatus = OrderStatus.StockConfirmed;
            Description = "All the items were confirmed with available stock.";
        }
    }

    public void Pay()
    {
        if (OrderStatus == OrderStatus.StockConfirmed)
        {
            AddDomainEvent(new OrderStatusChangedToPaidDomainEvent(Id, OrderItems, DateTime.UtcNow));

            OrderStatus = OrderStatus.Paid;
            Description = "The payment was performed at a simulated \"American Bank checking bank account ending on XX35071\"";
        }
    }

    public void Ship()
    {
        if (OrderStatus != OrderStatus.Paid)
        {
            StatusChangeException(OrderStatus.Shipped);
        }

        OrderStatus = OrderStatus.Shipped;
        Description = "The order was shipped.";
        AddDomainEvent(new OrderShippedDomainEvent(this, DateTime.UtcNow));
    }

    public void Cancel()
    {
        if (OrderStatus is OrderStatus.Paid or OrderStatus.Shipped)
        {
            StatusChangeException(OrderStatus.Cancelled);
        }

        OrderStatus = OrderStatus.Cancelled;
        Description = "The order was cancelled.";
        AddDomainEvent(new OrderCanceledDomainEvent(this, DateTime.UtcNow));
    }

    public void Cancel(IEnumerable<Guid> orderStockRejectedItems)
    {
        if (OrderStatus == OrderStatus.AwaitingValidation)
        {
            OrderStatus = OrderStatus.Cancelled;

            var itemsStockRejectedProductNames = OrderItems
                .Where(c => orderStockRejectedItems.Contains(c.ProductId))
                .Select(c => c.ProductName);

            var itemsStockRejectedDescription = string.Join(", ", itemsStockRejectedProductNames);
            Description = $"The product items don't have stock: ({itemsStockRejectedDescription}).";
        }
    }

    private void AddOrderStartedDomainEvent(
            Guid userId, string userName, int cardTypeId, string cardNumber,
            string cardSecurityNumber, string cardHolderName, DateTime cardExpiration)
    {
        var orderStartedDomainEvent = new OrderStartedDomainEvent(this, userId, userName, cardTypeId,
                                                                    cardNumber, cardSecurityNumber,
                                                                    cardHolderName, cardExpiration,
                                                                    DateTime.UtcNow);

        AddDomainEvent(orderStartedDomainEvent);
    }

    private void StatusChangeException(OrderStatus orderStatusToChange)
    {
        throw new OrderingDomainException($"Is not possible to change the order status from {OrderStatus} to {orderStatusToChange}.");
    }

    public decimal GetTotal() => OrderItems.Sum(o => o.Units * o.UnitPrice);
}
