namespace Webhooks.IntegrationTests;

public static class WebhookDbContextExtensions
{
    public static async Task SeedDatabase(this WebhooksDbContext dbContext)
    {
        if (!await dbContext.Subscriptions.AnyAsync())
        {
            //await dbContext.SaveChangesAsync();
        }
    }

}
