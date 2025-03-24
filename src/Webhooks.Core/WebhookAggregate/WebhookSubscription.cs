using SharedKernel;

namespace Webhooks.Core.WebhookAggregate;

public class WebhookSubscription : EntityBase<WebhookId>
{
    public WebhookSubscription(WebhookType type, DateTime date, Uri destinationUrl, string token, Guid userId)
        : base(new WebhookId(Guid.CreateVersion7()))
    {
        Type = type;
        Date = date;
        DestinationUrl = destinationUrl;
        Token = token;
        UserId = userId;
    }

    public WebhookType Type { get; private set; }

    public DateTime Date { get; private set; }

    public Uri DestinationUrl { get; private set; }

    public string Token { get; private set; }

    public Guid UserId { get; private set; }
}
