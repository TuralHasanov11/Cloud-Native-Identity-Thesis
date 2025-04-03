using Webhooks.Core.WebhookAggregate;

namespace Webhooks.UnitTests;

public class WebhookSubscriptionTests
{
    [Fact]
    public void Constructor_ShouldInitializeWebhookSubscription()
    {
        // Arrange
        var type = WebhookType.OrderPaid;
        var date = DateTime.UtcNow;
        var destinationUrl = new Uri("http://example.com/webhook");
        var token = "sample-token";
        var userId = new IdentityId("id");

        // Act
        var webhookSubscription = new WebhookSubscription(type, date, destinationUrl, token, userId);

        // Assert
        Assert.Equal(type, webhookSubscription.Type);
        Assert.Equal(date, webhookSubscription.Date);
        Assert.Equal(destinationUrl, webhookSubscription.DestinationUrl);
        Assert.Equal(token, webhookSubscription.Token);
        Assert.Equal(userId, webhookSubscription.UserId);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenDestinationUrlIsNull()
    {
        // Arrange
        var type = WebhookType.OrderPaid;
        var date = DateTime.UtcNow;
        Uri destinationUrl = null;
        var token = "sample-token";
        var userId = new IdentityId("id");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new WebhookSubscription(type, date, destinationUrl, token, userId));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenTokenIsNull()
    {
        // Arrange
        var type = WebhookType.OrderPaid;
        var date = DateTime.UtcNow;
        var destinationUrl = new Uri("http://example.com/webhook");
        string token = null;
        var userId = new IdentityId("id");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new WebhookSubscription(type, date, destinationUrl, token, userId));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenTokenIsEmpty()
    {
        // Arrange
        var type = WebhookType.OrderPaid;
        var date = DateTime.UtcNow;
        var destinationUrl = new Uri("http://example.com/webhook");
        var token = string.Empty;
        var userId = new IdentityId("id");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new WebhookSubscription(type, date, destinationUrl, token, userId));
    }
}
