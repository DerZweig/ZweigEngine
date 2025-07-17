namespace ZweigEngine.Common;

public sealed class ServiceProvider : IServiceProvider, IDisposable
{
    private readonly Dictionary<Type, object?> m_instances;
    private readonly Dictionary<Type, object?> m_collections;
    private readonly Stack<IDisposable>        m_disposables;

    internal ServiceProvider()
    {
        m_instances   = new Dictionary<Type, object?>();
        m_collections = new Dictionary<Type, object?>();
        m_disposables = new Stack<IDisposable>();
    }

    internal void AddSingleton(Type type, object instance)
    {
        m_instances.Add(type, instance);
    }

    internal void AddToCollection<TElement>(object element)
    {
        var key        = typeof(TElement);
        var collection = (List<TElement>?)m_collections.GetValueOrDefault(key);
        if (collection == null)
        {
            collection = new List<TElement>();
            m_instances.Add(typeof(IReadOnlyCollection<TElement>), collection);
            m_instances.Add(typeof(IReadOnlyList<TElement>), collection);
            m_instances.Add(typeof(IEnumerable<TElement>), collection);
            m_collections.Add(key, collection);
        }

        collection.Add((TElement)element);
    }

    internal void AddDestructor(IDisposable disposable)
    {
        m_disposables.Push(disposable);
    }

    public object? GetService(Type serviceType)
    {
        if (serviceType == typeof(IServiceProvider) || serviceType == typeof(ServiceProvider))
        {
            return this;
        }

        return m_instances.GetValueOrDefault(serviceType);
    }

    private void ReleaseUnmanagedResources()
    {
        m_collections.Clear();
        while (m_disposables.TryPop(out var disposable))
        {
            try
            {
                disposable.Dispose();
            }
            catch
            {
                //ignored
            }
        }

        m_disposables.Clear();
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~ServiceProvider()
    {
        ReleaseUnmanagedResources();
    }
}