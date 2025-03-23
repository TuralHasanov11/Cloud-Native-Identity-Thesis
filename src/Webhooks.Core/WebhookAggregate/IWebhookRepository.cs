using System.Linq.Expressions;
using SharedKernel;

namespace Webhooks.Core.WebhookAggregate;

public interface IWebhookRepository
{
    Task CreateAsync(WebhookSubscription subscription, CancellationToken cancellationToken = default);

    void Delete(WebhookSubscription subscription);

    Task<WebhookSubscription?> SingleOrDefaultAsync(
        Specification<WebhookSubscription> specification,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<WebhookSubscription>> ListAsync(
        Specification<WebhookSubscription> specification,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TResponse>> ListAsync<TResponse>(
        Specification<WebhookSubscription> specification,
        Expression<Func<WebhookSubscription, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class;
}
