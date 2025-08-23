using MediatR;

namespace SharedKernel;

public interface IDomainEvent : INotification;

public abstract record DomainEventBase(DateTime OccurredOnUtc) : IDomainEvent;
