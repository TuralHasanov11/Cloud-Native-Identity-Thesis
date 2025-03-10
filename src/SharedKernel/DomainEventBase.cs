namespace SharedKernel;

public interface IDomainEvent { }

public abstract record DomainEventBase : IDomainEvent
{
    public DateTime OccurredOnUtc { get; }
}
