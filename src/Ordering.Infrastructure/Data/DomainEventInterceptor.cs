using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ordering.Infrastructure.Data;

public class DomainEventInterceptor(IMediator mediator) : SaveChangesInterceptor
{
    private readonly IMediator _mediator = mediator;

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var entities = eventData.Context?.ChangeTracker
            .Entries<HasDomainEventsBase>()
            .Where(x => x.Entity.DomainEvents?.Count > 0);

        var domainEvents = entities?.SelectMany(x => x.Entity.DomainEvents).ToList();

        entities?.ToList().ForEach(entity => entity.Entity.ClearDomainEvents());

        if (domainEvents is not null)
        {
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
