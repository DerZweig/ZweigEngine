using System.Collections.Concurrent;

namespace ZweigEngine.Win32;

internal class Win32Synchronization
{
    private readonly ConcurrentQueue<Action> m_queue;
    private readonly Context                 m_context;

    public Win32Synchronization()
    {
        m_queue   = new ConcurrentQueue<Action>();
        m_context = new Context(m_queue);
    }

    public void ExecuteWithoutPending(Action action)
    {
        if (SynchronizationContext.Current != m_context)
        {
            var previous = SynchronizationContext.Current;

            try
            {
                SynchronizationContext.SetSynchronizationContext(m_context);
                action();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(previous);
            }
        }
        else
        {
            action();
        }
    }

    public void Execute(Action action)
    {
        if (SynchronizationContext.Current != m_context)
        {
            var previous = SynchronizationContext.Current;

            try
            {
                SynchronizationContext.SetSynchronizationContext(m_context);
                ExecutePending();
                action();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(previous);
            }
        }
        else
        {
            ExecutePending();
            action();
        }
    }

    private void ExecutePending()
    {
        while (m_queue.TryDequeue(out var work))
        {
            work();
        }
    }

    private class Context : SynchronizationContext
    {
        private readonly ConcurrentQueue<Action> m_queue;

        public Context(ConcurrentQueue<Action> queue)
        {
            m_queue = queue;
        }

        public override void Post(SendOrPostCallback d, object? state)
        {
            m_queue.Enqueue(() => d(state));
        }

        public override void Send(SendOrPostCallback d, object? state)
        {
            throw new NotSupportedException();
        }

        public override SynchronizationContext CreateCopy()
        {
            throw new NotSupportedException();
        }
    }
}