using System.Collections.Concurrent;

namespace Webhooks.Client.Services;

public class HooksRepository
{
    private readonly ConcurrentQueue<WebHookReceived> _data = new();
    private readonly ConcurrentDictionary<OnChangeSubscription, object?> _onChangeSubscriptions = new();

    public Task AddNew(WebHookReceived hook)
    {
        _data.Enqueue(hook);

        foreach (var subscription in _onChangeSubscriptions)
        {
            try
            {
                _ = subscription.Key.NotifyAsync();
            }
            catch (Exception)
            {
                // It's the subscriber's responsibility to report/handle any exceptions
                // that occur during their callback
            }
        }

        return Task.CompletedTask;
    }

    public Task<IEnumerable<WebHookReceived>> GetAll()
    {
        return Task.FromResult(_data.AsEnumerable());
    }

    public IDisposable Subscribe(Func<Task> callback)
    {
        var subscription = new OnChangeSubscription(callback, this);
        _onChangeSubscriptions.TryAdd(subscription, null);
        return subscription;
    }

    private sealed class OnChangeSubscription(Func<Task> callback, HooksRepository owner)
        : IDisposable
    {
        private readonly Func<Task> _callback = callback;
        private readonly HooksRepository _owner = owner;
        private bool _disposed;

        public Task NotifyAsync()
        {
            return _disposed ? throw new ObjectDisposedException(nameof(OnChangeSubscription)) : _callback();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _owner._onChangeSubscriptions.Remove(this, out _);
                }

                _disposed = true;
            }
        }
    }
}
