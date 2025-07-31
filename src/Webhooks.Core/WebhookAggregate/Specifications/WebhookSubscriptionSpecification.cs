using SharedKernel;

namespace Webhooks.Core.WebhookAggregate.Specifications;

public class WebhookSubscriptionSpecification
    : Specification<WebhookSubscription>
{
    public WebhookSubscriptionSpecification(IdentityId userId)
        : base(x => x.UserId == userId)
    {
    }

    public WebhookSubscriptionSpecification(WebhookType type)
        : base(subscription => subscription.Type == type)
    {
    }

    public WebhookSubscriptionSpecification(IdentityId userId, WebhookId id)
        : base(subscription => subscription.UserId == userId && subscription.Id == id)
    {
    }
}
