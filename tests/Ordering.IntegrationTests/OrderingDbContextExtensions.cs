namespace Ordering.IntegrationTests;

public static class OrderingDbContextExtensions
{
    public static async Task SeedDatabase(this OrderingDbContext dbContext)
    {
        if (!await dbContext.Orders.AnyAsync())
        {
            var orders = GetOrders();
            dbContext.Orders.AddRange(orders);

            await dbContext.SaveChangesAsync();
        }

        if (!await dbContext.Customers.AnyAsync())
        {
            var customers = GetCustomers();
            dbContext.Customers.AddRange(customers);
            await dbContext.SaveChangesAsync();
        }

        if (!await dbContext.PaymentMethods.AnyAsync())
        {
            var paymentMethods = GetPaymentMethods();
            dbContext.PaymentMethods.AddRange(paymentMethods);
            await dbContext.SaveChangesAsync();
        }

        if (!await dbContext.CardTypes.AnyAsync())
        {
            var cardTypes = GetCardTypes();
            dbContext.CardTypes.AddRange(cardTypes);
            await dbContext.SaveChangesAsync();
        }
    }

    private static CardType[] GetCardTypes()
    {
        return [
        ];
    }

    private static List<Order> GetOrders()
    {
        return
        [

        ];
    }

    private static List<Customer> GetCustomers()
    {
        return
        [

        ];
    }

    private static List<PaymentMethod> GetPaymentMethods()
    {
        return
        [

        ];
    }
}
