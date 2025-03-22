using SharedKernel;

namespace Webhooks.Core.WebhookAggregate;

public class WebhookSubscription : EntityBase<WebhookId>
{
    public WebhookSubscription(WebhookType type, DateTime date, Uri destUrl, string token, Guid userId)
        : base(new WebhookId(Guid.CreateVersion7()))
    {
        Type = type;
        Date = date;
        DestUrl = destUrl;
        Token = token;
        UserId = userId;
    }

    public WebhookType Type { get; private set; }

    public DateTime Date { get; private set; }

    public Uri DestUrl { get; private set; }

    public string Token { get; private set; }

    public Guid UserId { get; private set; }
}
