namespace Outbox;

public sealed class OutboxMessage
{
    public OutboxMessage(IntegrationEvent @event, DateTime createdOnUtc, Guid transactionId)
    {
        Id = @event.Id;
        Type = @event.GetType().FullName!;
        CreatedOnUtc = createdOnUtc;
        Content = JsonSerializer.Serialize(@event, @event.GetType(), IndentedOptions);
        State = EventState.NotPublished;
        TimesSent = 0;
        TransactionId = transactionId;
    }

    private OutboxMessage() { }

    private static readonly JsonSerializerOptions IndentedOptions = new() { WriteIndented = true };

    private static readonly JsonSerializerOptions CaseInsensitiveOptions = new() { PropertyNameCaseInsensitive = true };

    public Guid Id { get; }

    public string Type { get; }

    public string Content { get; }

    public IntegrationEvent IntegrationEvent { get; private set; }

    public EventState State { get; set; }

    public int TimesSent { get; set; }

    public DateTime CreatedOnUtc { get; }

    public DateTime? ProcessedOnUtc { get; private set; }

    public Guid TransactionId { get; private set; }

    public string? Error { get; private set; }

    public void Process(DateTime processedOnUtc)
    {
        ProcessedOnUtc = processedOnUtc;
    }

    public void Fail(string error, DateTime processedOnUtc)
    {
        ProcessedOnUtc = processedOnUtc;
        Error = error;
    }

    public OutboxMessage DeserializeJsonContent(Type type)
    {
        IntegrationEvent = JsonSerializer.Deserialize(Content, type, CaseInsensitiveOptions) as IntegrationEvent;
        return this;
    }
}
