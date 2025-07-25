using ZweigEngine.Common.Utility;
using ZweigEngine.Common.Utility.Concurrency;

namespace ZweigEngine.Common.Services.Messages;


internal class MessageSubscription<THandler> : DisposableObject where THandler : class
{
    private readonly WeakReference<THandler> m_reference;
    private          ConcurrentBoolean       m_active;

    public MessageSubscription(THandler handler)
    {
        m_reference = new WeakReference<THandler>(handler);
        m_active    = new ConcurrentBoolean(true);
    }

    public bool IsNotActive()
    {
        return m_active.Read() == false;
    }

    public bool TryInvoke(Action<THandler> message)
    {
        if (m_active.Read() && m_reference.TryGetTarget(out var target) && m_active.Read())
        {
            message(target);
            return true;
        }

        return false;
    }

    protected override void ReleaseUnmanagedResources()
    {
        m_active.Exchange(false);
    }
}