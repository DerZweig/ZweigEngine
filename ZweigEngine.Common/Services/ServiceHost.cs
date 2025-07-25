using System.Reflection;
using ZweigEngine.Common.Utility;

namespace ZweigEngine.Common.Services;

public class ServiceHost : DisposableObject
{
    private delegate bool InitializerDelegate(ServiceHost host);

    private readonly IReadOnlyList<InitializerDelegate> m_initializers;
    private readonly Dictionary<Type, object>           m_instances;
    private readonly Stack<IDisposable>                 m_disposables;
    private readonly IServiceProvider                   m_services;

    private ServiceHost(IReadOnlyList<InitializerDelegate> initializers)
    {
        m_initializers = initializers;
        m_instances    = new Dictionary<Type, object>();
        m_disposables  = new Stack<IDisposable>();
        m_services     = new ServiceProviderProxy(m_instances);
        m_instances.Add(typeof(IServiceProvider), m_services);
    }

    protected override void ReleaseUnmanagedResources()
    {
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

        m_instances.Clear();
    }

    public IServiceProvider Services => m_services;

    public void InitializeServices()
    {
        var pending    = new Queue<InitializerDelegate>();
        var unresolved = new Queue<InitializerDelegate>();

        foreach (var initializer in m_initializers)
        {
            pending.Enqueue(initializer);
        }

        while (pending.Count != 0)
        {
            var beforeCount = pending.Count;
            while (pending.TryDequeue(out var work))
            {
                if (!work(this))
                {
                    unresolved.Enqueue(work);
                }
            }

            if (beforeCount == unresolved.Count)
            {
                throw new Exception("Stopped processing service initializers, check for circular dependencies.");
            }

            while (unresolved.TryDequeue(out var work))
            {
                pending.Enqueue(work);
            }
        }
    }

    public static ServiceHost Create(ServiceConfiguration configuration)
    {
        var initializers = new List<InitializerDelegate>();
        var knownTypes   = new HashSet<Type>();
        var instances    = new HashSet<Type>();

        knownTypes.Add(typeof(IServiceProvider));
        
        foreach (var step in configuration.Enumerate())
        {
            knownTypes.Add(step.type);
            if (step.alias != null)
            {
                knownTypes.Add(step.alias);
            }
        }

        foreach (var step in configuration.Enumerate())
        {
            knownTypes.Add(step.type);
            if (step.alias != null)
            {
                knownTypes.Add(step.alias);
            }
        }

        foreach (var step in configuration.Enumerate())
        {
            if (instances.Add(step.type))
            {
                initializers.Add(CreateConstructor(step.type, knownTypes));
            }

            if (step.alias != null && step.alias != step.type)
            {
                initializers.Add(CreateBinding(step.type, step.alias));
            }
        }

        return new ServiceHost(initializers);
    }

    private static InitializerDelegate CreateConstructor(Type type, IReadOnlySet<Type> knownTypes)
    {
        var selectedConstructor = (ConstructorInfo?)null;
        var selectedParameters  = Array.Empty<Type>();
        var typeConstructors    = type.GetConstructors();

        foreach (var constructor in typeConstructors)
        {
            var parameters = constructor.GetParameters();
            if (parameters.Length <= selectedParameters.Length && selectedConstructor != null)
            {
                continue;
            }

            var parameterTypes = parameters.Select(x => x.ParameterType).ToArray();
            if (parameterTypes.Any(x => !knownTypes.Contains(x)))
            {
                continue;
            }

            selectedConstructor = constructor;
            selectedParameters  = parameterTypes;
        }

        if (selectedConstructor == null)
        {
            throw new Exception($"Couldn't find suitable constructor for type {type.Name}.");
        }

        return services =>
        {
            var parameters = selectedParameters.Select(x => services.m_instances.GetValueOrDefault(x)).ToArray();
            if (parameters.Any(x => x == null))
            {
                return false;
            }

            var instance = selectedConstructor.Invoke(parameters);
            if (instance is IDisposable disposable)
            {
                //add disposable before possible exception from dictionary
                services.m_disposables.Push(disposable);
            }

            services.m_instances.Add(type, instance);
            return true;
        };
    }

    private static InitializerDelegate CreateBinding(Type type, Type alias)
    {
        return services =>
        {
            var typeInstance = services.m_instances.GetValueOrDefault(type);
            if (typeInstance == null)
            {
                return false;
            }

            var aliasInstance = services.m_instances.GetValueOrDefault(alias);
            if (aliasInstance != null)
            {
                return ReferenceEquals(aliasInstance, typeInstance);
            }

            services.m_instances.Add(alias, typeInstance);
            return true;
        };
    }
}