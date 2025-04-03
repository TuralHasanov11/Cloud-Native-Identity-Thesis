namespace Outbox.Services;

public interface IOutboxProcessor
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}
