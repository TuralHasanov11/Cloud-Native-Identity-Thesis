namespace Outbox.Services;

public interface IOutboxService
{
    Task<IEnumerable<OutboxMessage>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId);

    Task SaveEventAsync(IntegrationEvent message);

    Task MarkEventAsPublishedAsync(Guid eventId);

    Task MarkEventAsInProgressAsync(Guid eventId);

    Task MarkEventAsFailedAsync(Guid eventId);
}
