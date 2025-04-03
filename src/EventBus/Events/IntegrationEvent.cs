namespace EventBus.Events;

public record IntegrationEvent
{
    public IntegrationEvent()
    {
        Id = Guid.CreateVersion7();
        CreationDate = DateTime.UtcNow;
    }

    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }
}
