using Webhooks.Core.WebhookAggregate.Specifications;

namespace Webhooks.IntegrationTests.Webhooks;

public class WebhookSubscriptionRepositoryTests : BaseIntegrationTest
{
    private readonly IWebhookSubscriptionRepository _repository;
    private readonly CancellationToken _cancellationToken = TestContext.Current.CancellationToken;

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

        await _repository.CreateAsync(subscription, _cancellationToken);
        await _repository.SaveChangesAsync(_cancellationToken);

        var createdSubscription = await DbContext.Subscriptions.FirstOrDefaultAsync(s => s.Id == subscription.Id, _cancellationToken);
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

        await _repository.CreateAsync(subscription, _cancellationToken);
        await _repository.SaveChangesAsync(_cancellationToken);

        // Act
        _repository.Delete(subscription);
        await _repository.SaveChangesAsync(_cancellationToken);

        // Assert
        var deletedSubscription = await DbContext.Subscriptions.FirstOrDefaultAsync(s => s.Id == subscription.Id, _cancellationToken);
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

        await _repository.CreateAsync(subscription1, _cancellationToken);
        await _repository.CreateAsync(subscription2, _cancellationToken);
        await _repository.SaveChangesAsync(_cancellationToken);

        var specification = new WebhookSubscriptionSpecification(WebhookType.OrderPaid);

        // Act
        var subscriptions = await _repository.ListAsync(specification, _cancellationToken);

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

        await repository.CreateAsync(subscription, _cancellationToken);
        await repository.SaveChangesAsync(_cancellationToken);
        var specification = new WebhookSubscriptionSpecification(subscription.UserId, subscription.Id);

        // Act
        var result = await repository.SingleOrDefaultAsync(specification, _cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(subscription.Id, result.Id);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnNull_WhenSubscriptionDoesNotExist()
    {
        // Arrange
        var specification = new WebhookSubscriptionSpecification(
            new IdentityId(IdentityExtensions.GenerateId()),
            new WebhookId(Guid.CreateVersion7()));

        // Act
        var result = await _repository.SingleOrDefaultAsync(specification, _cancellationToken);

        // Assert
        Assert.Null(result);
    }
}
