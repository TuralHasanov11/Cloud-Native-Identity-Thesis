﻿namespace Outbox.Services;

public class OutboxService<TContext> : IOutboxService, IDisposable
    where TContext : DbContext
{
    private readonly TContext _context;
    private readonly Type[] _eventTypes;
    private volatile bool _disposedValue;

    public OutboxService(TContext context, Assembly assembly)
    {
        _context = context;
        _eventTypes = [.. assembly
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(IntegrationEvent)))];
    }

    public async Task<IEnumerable<OutboxMessage>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId)
    {
        var result = await _context.Set<OutboxMessage>()
            .Where(e => e.TransactionId == transactionId && e.State == EventState.NotPublished)
            .ToListAsync();

        return result.Count != 0
            ? result.OrderBy(o => o.CreatedOnUtc)
                .Select(e => e.DeserializeJsonContent(_eventTypes.First(t => t.FullName == e.Type)))
            : [];
    }

    public Task SaveEventAsync(IntegrationEvent message)
    {
        var eventLogEntry = new OutboxMessage(message, DateTime.UtcNow, Guid.NewGuid());

        _context.Set<OutboxMessage>().Add(eventLogEntry);

        return _context.SaveChangesAsync();
    }

    public Task MarkEventAsPublishedAsync(Guid eventId)
    {
        return UpdateEventStatus(eventId, EventState.Published);
    }

    public Task MarkEventAsInProgressAsync(Guid eventId)
    {
        return UpdateEventStatus(eventId, EventState.InProgress);
    }

    public Task MarkEventAsFailedAsync(Guid eventId)
    {
        return UpdateEventStatus(eventId, EventState.PublishedFailed);
    }

    private Task UpdateEventStatus(Guid eventId, EventState status)
    {
        var eventLogEntry = _context.Set<OutboxMessage>().Single(ie => ie.Id == eventId);

        if (eventLogEntry is null)
        {
            return Task.CompletedTask;
        }

        eventLogEntry.State = status;

        if (status == EventState.InProgress)
        {
            eventLogEntry.TimesSent++;
        }

        return _context.SaveChangesAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _context.Dispose();
            }


            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
