using Webhooks.Core.WebhookAggregate.Specifications;

namespace Webhooks.IntegrationTests.Webhooks;


public class WebhookSubscriptionRepositoryTests : IClassFixture<WebhooksFactory>
{
    private readonly WebhooksFactory _factory;

    public WebhookSubscriptionRepositoryTests(WebhooksFactory factory)
    {
        _factory = factory;
    }

    [Fact(Skip = "Waiting")]
    public async Task CreateAsync_ShouldAddSubscription()
    {
        var dbContext = _factory.Services.GetRequiredService<WebhooksDbContext>();
        await dbContext.SeedDatabase();

        var repository = new WebhookSubscriptionRepository(dbContext);
        var subscription = new WebhookSubscription(
            WebhookType.OrderPaid,
            DateTime.UtcNow,
            new Uri("http://example.com/webhook"),
            "sample-token",
            new IdentityId(IdentityExtensions.GenerateId()));

        await repository.CreateAsync(subscription);
        await repository.SaveChangesAsync();

        var createdSubscription = await dbContext.Subscriptions.FindAsync(subscription.Id);
        Assert.NotNull(createdSubscription);
    }

    [Fact(Skip = "Waiting")]
    public async Task Delete_ShouldRemoveSubscription()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<WebhooksDbContext>();
        await dbContext.SeedDatabase();

        var repository = new WebhookSubscriptionRepository(dbContext);
        var subscription = new WebhookSubscription(
            WebhookType.OrderPaid,
            DateTime.UtcNow,
            new Uri("http://example.com/webhook"),
            "sample-token",
            new IdentityId(IdentityExtensions.GenerateId()));

        await repository.CreateAsync(subscription);
        await repository.SaveChangesAsync();

        // Act
        repository.Delete(subscription);
        await repository.SaveChangesAsync();

        // Assert
        var deletedSubscription = await dbContext.Subscriptions.FindAsync(subscription.Id);
        Assert.Null(deletedSubscription);
    }

    [Fact(Skip = "Waiting")]
    public async Task ListAsync_ShouldReturnSubscriptions()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<WebhooksDbContext>();
        await dbContext.SeedDatabase();

        var repository = new WebhookSubscriptionRepository(dbContext);

        var subscription1 = new WebhookSubscription(
            WebhookType.OrderPaid,
            DateTime.UtcNow,
            new Uri("http://example.com/webhook1"),
            "sample-token1",
            new IdentityId(IdentityExtensions.GenerateId()));

        var subscription2 = new WebhookSubscription(
            WebhookType.OrderPaid,
            DateTime.UtcNow,
            new Uri("http://example.com/webhook2"),
            "sample-token2",
            new IdentityId(IdentityExtensions.GenerateId()));

        await repository.CreateAsync(subscription1);
        await repository.CreateAsync(subscription2);
        await repository.SaveChangesAsync();

        var specification = new GetWebhookSubscriptionSpecification(WebhookType.OrderPaid);

        // Act
        var subscriptions = await repository.ListAsync(specification);

        // Assert
        Assert.Contains(subscriptions, s => s.Id == subscription1.Id);
        Assert.Contains(subscriptions, s => s.Id == subscription2.Id);
    }

    [Fact(Skip = "Waiting")]
    public async Task SingleOrDefaultAsync_ShouldReturnSubscription()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<WebhooksDbContext>();
        await dbContext.SeedDatabase();

        var repository = new WebhookSubscriptionRepository(dbContext);
        var subscription = new WebhookSubscription(
            WebhookType.OrderPaid,
            DateTime.UtcNow,
            new Uri("http://example.com/webhook"),
            "sample-token",
            new IdentityId(IdentityExtensions.GenerateId()));

        await repository.CreateAsync(subscription);
        await repository.SaveChangesAsync();
        var specification = new GetWebhookSubscriptionSpecification(subscription.UserId, subscription.Id);

        // Act
        var result = await repository.SingleOrDefaultAsync(specification);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(subscription.Id, result.Id);
    }

    [Fact(Skip = "Waiting")]
    public async Task SingleOrDefaultAsync_ShouldReturnNull_WhenSubscriptionDoesNotExist()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<WebhooksDbContext>();
        await dbContext.SeedDatabase();

        var repository = new WebhookSubscriptionRepository(dbContext);
        var specification = new GetWebhookSubscriptionSpecification(
            new IdentityId(IdentityExtensions.GenerateId()),
            new WebhookId(Guid.CreateVersion7()));

        // Act
        var result = await repository.SingleOrDefaultAsync(specification);

        // Assert
        Assert.Null(result);
    }
}
