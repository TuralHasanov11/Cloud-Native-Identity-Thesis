using SharedKernel;

namespace Webhooks.Core.WebhookAggregate.Specifications;

public class GetWebhookSubscriptionsSpecification
    : Specification<WebhookSubscription>
{
    public GetWebhookSubscriptionsSpecification(WebhookType type)
    {
        AddCriteria(x => x.Type == type);
    }

    public GetWebhookSubscriptionsSpecification(IdentityId userId)
    {
        AddCriteria(x => x.UserId == userId);
    }
}
