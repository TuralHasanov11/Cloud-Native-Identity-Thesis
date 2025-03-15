namespace SharedKernel;

public interface IOutboxProcessor
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}
