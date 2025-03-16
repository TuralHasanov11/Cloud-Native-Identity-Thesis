using MassTransit;

namespace EventBus.Abstractions;

public interface IIntegrationEventHandler<in TIntegrationEvent>
    : IConsumer<TIntegrationEvent>
    where TIntegrationEvent : IntegrationEvent;

public interface IIntegrationEventHandler : IConsumer;
