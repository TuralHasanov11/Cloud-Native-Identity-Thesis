using SharedKernel;

namespace Webhooks.Core.WebhookAggregate;

public class WebhookSubscription : EntityBase<WebhookId>
{
    public WebhookSubscription(WebhookType type, DateTime date, Uri destinationUrl, string token, IdentityId userId)
        : base(new WebhookId(Guid.CreateVersion7()))
    {
        Type = type;
        Date = date;
        DestinationUrl = destinationUrl
            ?? throw new ArgumentNullException(nameof(destinationUrl), "The destination URL cannot be null.");
        Token = string.IsNullOrWhiteSpace(token)
            ? throw new ArgumentException("The token cannot be null or empty.", nameof(token))
            : token;
        UserId = userId;
    }

    public WebhookType Type { get; private set; }

    public DateTime Date { get; private set; }

    public Uri DestinationUrl { get; private set; }

    public string Token { get; private set; }

    public IdentityId UserId { get; private set; }
}
