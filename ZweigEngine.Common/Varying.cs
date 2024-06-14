namespace ZweigEngine.Common;

internal sealed class Varying<TInterface> : IDisposable, IVarying<TInterface> where TInterface : class
{
    private readonly Dictionary<string, Func<object>> m_factories;

    public Varying()
    {
        m_factories = new Dictionary<string, Func<object>>();
    }

    public void AddOption(string name, Func<object> factory)
    {
        m_factories.Add(name, factory);
    }

    public TInterface? Current { get; private set; }

    public IEnumerable<string> EnumerateOptions()
    {
        return m_factories.Keys.ToArray();
    }

    public void Activate(string optionName)
    {
        Deactivate();
        if (m_factories.TryGetValue(optionName, out var factory))
        {
            Current = (TInterface)factory();
        }
    }

    public void Deactivate()
    {
        if (Current is IDisposable disposable)
        {
            disposable.Dispose();
        }

        Current = null;
    }

    private void ReleaseUnmanagedResources()
    {
        Deactivate();
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~Varying()
    {
        ReleaseUnmanagedResources();
    }
}