using System.Linq.Expressions;
using SharedKernel;
using Webhooks.Infrastructure.Data;

namespace Webhooks.Infrastructure.Repositories;

public class WebhookSubscriptionRepository(WebhooksDbContext dbContext) : IWebhookSubscriptionRepository
{
    public async Task CreateAsync(
        WebhookSubscription subscription,
        CancellationToken cancellationToken = default)
    {
        await dbContext.Subscriptions.AddAsync(subscription, cancellationToken);
    }

    public void Delete(WebhookSubscription subscription)
    {
        dbContext.Subscriptions.Remove(subscription);
    }

    public async Task<IEnumerable<WebhookSubscription>> ListAsync(
        Specification<WebhookSubscription> specification,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Subscriptions
            .GetQuery(specification)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TResponse>> ListAsync<TResponse>(
        Specification<WebhookSubscription> specification,
        Expression<Func<WebhookSubscription, TResponse>> mapper,
        CancellationToken cancellationToken = default)
        where TResponse : class
    {
        return await dbContext.Subscriptions
            .GetQuery(specification)
            .Select(mapper)
            .ToListAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<WebhookSubscription?> SingleOrDefaultAsync(
        Specification<WebhookSubscription> specification,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Subscriptions.GetQuery(specification).FirstOrDefaultAsync(cancellationToken);
    }
}
