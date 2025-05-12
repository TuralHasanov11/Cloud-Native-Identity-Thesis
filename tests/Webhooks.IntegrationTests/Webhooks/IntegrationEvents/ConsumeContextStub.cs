using System.Diagnostics.CodeAnalysis;
using MassTransit;

namespace Webhooks.IntegrationTests.Webhooks.IntegrationEvents
{

    public class ConsumeContextStub<T> : ConsumeContext<T> where T : class
    {
        public ConsumeContextStub(T message)
        {
            Message = message;
        }

        public T Message { get; }

        public ReceiveContext ReceiveContext => throw new NotImplementedException();

        public SerializerContext SerializerContext => throw new NotImplementedException();

        public Task ConsumeCompleted => throw new NotImplementedException();

        public IEnumerable<string> SupportedMessageTypes => throw new NotImplementedException();

        public CancellationToken CancellationToken => throw new NotImplementedException();

        public Guid? MessageId => throw new NotImplementedException();

        public Guid? RequestId => throw new NotImplementedException();

        public Guid? CorrelationId => throw new NotImplementedException();

        public Guid? ConversationId => throw new NotImplementedException();

        public Guid? InitiatorId => throw new NotImplementedException();

        public DateTime? ExpirationTime => throw new NotImplementedException();

        public Uri? SourceAddress => throw new NotImplementedException();

        public Uri? DestinationAddress => throw new NotImplementedException();

        public Uri? ResponseAddress => throw new NotImplementedException();

        public Uri? FaultAddress => throw new NotImplementedException();

        public DateTime? SentTime => throw new NotImplementedException();

        public Headers Headers => throw new NotImplementedException();

        public HostInfo Host => throw new NotImplementedException();

        public void AddConsumeTask(Task task)
        {
            throw new NotImplementedException();
        }

        public T1 AddOrUpdatePayload<T1>(PayloadFactory<T1> addFactory, UpdatePayloadFactory<T1> updateFactory) where T1 : class
        {
            throw new NotImplementedException();
        }

        public ConnectHandle ConnectPublishObserver(IPublishObserver observer)
        {
            throw new NotImplementedException();
        }

        public ConnectHandle ConnectSendObserver(ISendObserver observer)
        {
            throw new NotImplementedException();
        }

        public T1 GetOrAddPayload<T1>(PayloadFactory<T1> payloadFactory) where T1 : class
        {
            throw new NotImplementedException();
        }

        public Task<ISendEndpoint> GetSendEndpoint(Uri address)
        {
            throw new NotImplementedException();
        }

        public bool HasMessageType(Type messageType)
        {
            throw new NotImplementedException();
        }

        public bool HasPayloadType(Type payloadType)
        {
            throw new NotImplementedException();
        }

        public Task NotifyConsumed(TimeSpan duration, string consumerType)
        {
            throw new NotImplementedException();
        }

        public Task NotifyConsumed<T1>(ConsumeContext<T1> context, TimeSpan duration, string consumerType) where T1 : class
        {
            throw new NotImplementedException();
        }

        public Task NotifyFaulted(TimeSpan duration, string consumerType, Exception exception)
        {
            throw new NotImplementedException();
        }

        public Task NotifyFaulted<T1>(ConsumeContext<T1> context, TimeSpan duration, string consumerType, Exception exception) where T1 : class
        {
            throw new NotImplementedException();
        }

        public Task Publish<T1>(T1 message, CancellationToken cancellationToken = default) where T1 : class
        {
            throw new NotImplementedException();
        }

        public Task Publish<T1>(T1 message, IPipe<PublishContext<T1>> publishPipe, CancellationToken cancellationToken = default) where T1 : class
        {
            throw new NotImplementedException();
        }

        public Task Publish<T1>(T1 message, IPipe<PublishContext> publishPipe, CancellationToken cancellationToken = default) where T1 : class
        {
            throw new NotImplementedException();
        }

        public Task Publish(object message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Publish(object message, IPipe<PublishContext> publishPipe, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Publish(object message, Type messageType, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Publish(object message, Type messageType, IPipe<PublishContext> publishPipe, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Publish<T1>(object values, CancellationToken cancellationToken = default) where T1 : class
        {
            throw new NotImplementedException();
        }

        public Task Publish<T1>(object values, IPipe<PublishContext<T1>> publishPipe, CancellationToken cancellationToken = default) where T1 : class
        {
            throw new NotImplementedException();
        }

        public Task Publish<T1>(object values, IPipe<PublishContext> publishPipe, CancellationToken cancellationToken = default) where T1 : class
        {
            throw new NotImplementedException();
        }

        public void Respond<T1>(T1 message) where T1 : class
        {
            throw new NotImplementedException();
        }

        public Task RespondAsync<T1>(T1 message) where T1 : class
        {
            throw new NotImplementedException();
        }

        public Task RespondAsync<T1>(T1 message, IPipe<SendContext<T1>> sendPipe) where T1 : class
        {
            throw new NotImplementedException();
        }

        public Task RespondAsync<T1>(T1 message, IPipe<SendContext> sendPipe) where T1 : class
        {
            throw new NotImplementedException();
        }

        public Task RespondAsync(object message)
        {
            throw new NotImplementedException();
        }

        public Task RespondAsync(object message, Type messageType)
        {
            throw new NotImplementedException();
        }

        public Task RespondAsync(object message, IPipe<SendContext> sendPipe)
        {
            throw new NotImplementedException();
        }

        public Task RespondAsync(object message, Type messageType, IPipe<SendContext> sendPipe)
        {
            throw new NotImplementedException();
        }

        public Task RespondAsync<T1>(object values) where T1 : class
        {
            throw new NotImplementedException();
        }

        public Task RespondAsync<T1>(object values, IPipe<SendContext<T1>> sendPipe) where T1 : class
        {
            throw new NotImplementedException();
        }

        public Task RespondAsync<T1>(object values, IPipe<SendContext> sendPipe) where T1 : class
        {
            throw new NotImplementedException();
        }

        public bool TryGetMessage<T1>([NotNullWhen(true)] out ConsumeContext<T1>? consumeContext) where T1 : class
        {
            throw new NotImplementedException();
        }

        public bool TryGetPayload<T1>([NotNullWhen(true)] out T1 payload) where T1 : class
        {
            throw new NotImplementedException();
        }
        // Implement other members of ConsumeContext<T> as needed
    }
}
