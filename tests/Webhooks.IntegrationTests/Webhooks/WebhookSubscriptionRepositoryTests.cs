using Webhooks.Core.WebhookAggregate.Specifications;

namespace Webhooks.IntegrationTests.Webhooks;


public class WebhookSubscriptionRepositoryTests : BaseIntegrationTest
{
    private readonly IWebhookSubscriptionRepository _repository;
    private static readonly CancellationTokenSource _cancellationTokenSource = new(TimeSpan.FromSeconds(30));

    public WebhookSubscriptionRepositoryTests(WebhooksFactory factory)
        : base(factory)
    {
        _repository = factory.Services.GetRequiredService<IWebhookSubscriptionRepository>();
    }

    [Fact]
    public async Task CreateAsync_ShouldAddSubscription()
    {
        var subscription = new WebhookSubscription(
            WebhookType.OrderPaid,
            DateTime.UtcNow,
            new Uri("http://example.com/webhook"),
            "sample-token",
            new IdentityId(IdentityExtensions.GenerateId()));

        await _repository.CreateAsync(subscription, _cancellationTokenSource.Token);
        await _repository.SaveChangesAsync(_cancellationTokenSource.Token);

        var createdSubscription = await DbContext.Subscriptions.FindAsync(subscription.Id);
        Assert.NotNull(createdSubscription);
    }

    [Fact]
    public async Task Delete_ShouldRemoveSubscription()
    {
        // Arrange
        var subscription = new WebhookSubscription(
            WebhookType.OrderPaid,
            DateTime.UtcNow,
            new Uri("http://example.com/webhook"),
            "sample-token",
            new IdentityId(IdentityExtensions.GenerateId()));

        await _repository.CreateAsync(subscription);
        await _repository.SaveChangesAsync();

        // Act
        _repository.Delete(subscription);
        await _repository.SaveChangesAsync();

        // Assert
        var deletedSubscription = await DbContext.Subscriptions.FindAsync(subscription.Id);
        Assert.Null(deletedSubscription);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnSubscriptions()
    {
        // Arrange
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

        await _repository.CreateAsync(subscription1);
        await _repository.CreateAsync(subscription2);
        await _repository.SaveChangesAsync();

        var specification = new GetWebhookSubscriptionSpecification(WebhookType.OrderPaid);

        // Act
        var subscriptions = await _repository.ListAsync(specification);

        // Assert
        Assert.Contains(subscriptions, s => s.Id == subscription1.Id);
        Assert.Contains(subscriptions, s => s.Id == subscription2.Id);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnSubscription()
    {
        // Arrange
        var repository = new WebhookSubscriptionRepository(DbContext);
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

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnNull_WhenSubscriptionDoesNotExist()
    {
        // Arrange
        var specification = new GetWebhookSubscriptionSpecification(
            new IdentityId(IdentityExtensions.GenerateId()),
            new WebhookId(Guid.CreateVersion7()));

        // Act
        var result = await _repository.SingleOrDefaultAsync(specification);

        // Assert
        Assert.Null(result);
    }
}
