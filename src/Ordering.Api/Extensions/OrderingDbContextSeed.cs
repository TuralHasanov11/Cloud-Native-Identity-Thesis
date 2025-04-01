namespace Ordering.Api.Extensions;

public class OrderingDbContextSeed : IDbSeeder<OrderingDbContext>
{
    public async Task SeedAsync(OrderingDbContext context)
    {
        if (!context.CardTypes.Any())
        {
            context.CardTypes.AddRange(GetPredefinedCardTypes());

            await context.SaveChangesAsync();
        }

        await context.SaveChangesAsync();
    }

    private static IEnumerable<CardType> GetPredefinedCardTypes()
    {
        yield return CardType.Create("Amex");
        yield return CardType.Create("Visa");
        yield return CardType.Create("MasterCard");
    }
}
