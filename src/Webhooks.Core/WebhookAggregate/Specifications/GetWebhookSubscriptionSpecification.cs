using SharedKernel;

namespace Webhooks.Core.WebhookAggregate.Specifications;

public class GetWebhookSubscriptionSpecification
    : Specification<WebhookSubscription>
{
    public GetWebhookSubscriptionSpecification(WebhookType type)
        : base(subscription => subscription.Type == type)
    {
    }

    public GetWebhookSubscriptionSpecification(IdentityId userId, WebhookId id)
        : base(subscription => subscription.UserId == userId && subscription.Id == id)
    {
    }
}
