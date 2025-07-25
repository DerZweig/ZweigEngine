namespace ZweigEngine.Common.Services.Messages;

internal class MessageBus : IMessageBus
{
    private readonly ReaderWriterLockSlim     m_sync;
    private readonly Dictionary<Type, object> m_lists;

    public MessageBus()
    {
        m_sync  = new ReaderWriterLockSlim();
        m_lists = new Dictionary<Type, object>();
    }

    public IDisposable Subscribe<THandler>(THandler handler) where THandler : class
    {
        var subscription = new MessageSubscription<THandler>(handler);
        try
        {
            m_sync.EnterWriteLock();
            if (m_lists.GetValueOrDefault(typeof(THandler)) is not List<MessageSubscription<THandler>> list)
            {
                list = [];
                m_lists.Add(typeof(THandler), list);
            }

            list.Add(subscription);
        }
        finally
        {
            m_sync.ExitWriteLock();
        }

        return subscription;
    }

    public void Publish<THandler>(Action<THandler> message) where THandler : class
    {
        var copied = (MessageSubscription<THandler>[]?)null;
        try
        {
            m_sync.EnterReadLock();
            if (m_lists.GetValueOrDefault(typeof(THandler)) is List<MessageSubscription<THandler>> list)
            {
                copied = list.ToArray();
            }
        }
        finally
        {
            m_sync.ExitReadLock();
        }

        if (copied == null || copied.Count(x => x.TryInvoke(message)) != copied.Length)
        {
            return;
        }

        try
        {
            m_sync.EnterWriteLock();
            if (m_lists.GetValueOrDefault(typeof(THandler)) is List<MessageSubscription<THandler>> list)
            {
                list.RemoveAll(x => x.IsNotActive());
            }
        }
        finally
        {
            m_sync.ExitWriteLock();
        }
    }
}