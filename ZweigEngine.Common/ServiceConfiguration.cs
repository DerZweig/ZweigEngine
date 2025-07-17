using System.Reflection;
using ZweigEngine.Common.Utility.Extensions;

namespace ZweigEngine.Common;

public class ServiceConfiguration
{
    private readonly HashSet<Type> m_interfaces;
    private readonly HashSet<Type> m_implementations;

    private readonly List<Func<ServiceProvider, bool>> m_steps;

    public ServiceConfiguration()
    {
        m_interfaces      = new HashSet<Type>();
        m_implementations = new HashSet<Type>();
        m_steps           = new List<Func<ServiceProvider, bool>>();
    }

    public void AddSingleton<TImplementation>()
    {
        var type = typeof(TImplementation);
        m_interfaces.Add(type);
        m_implementations.Add(type);
    }

    public void AddSingleton<TInterface, TImplementation>() where TImplementation : TInterface
    {
        var key   = typeof(TInterface);
        var value = typeof(TImplementation);

        m_interfaces.Add(key);
        m_implementations.Add(value);
        if (key == value)
        {
            return;
        }

        m_interfaces.Add(value);
        m_steps.Add(services =>
        {
            if (services.TryGetOptionalService<TInterface>(out _))
            {
                return true;
            }

            if (!services.TryGetOptionalService<TImplementation>(out var instance))
            {
                return false;
            }

            services.AddSingleton(key, instance!);
            return true;
        });
    }

    public void AddVariant<TElementInterface, TElementImplementation>() where TElementImplementation : TElementInterface
    {
        var key   = typeof(TElementInterface);
        var value = typeof(TElementImplementation);

        m_interfaces.Add(key);
        m_implementations.Add(value);
        if (key == value)
        {
            return;
        }

        m_interfaces.Add(value);
        m_steps.Add(services =>
        {
            if (services.TryGetOptionalService<TElementInterface>(out _))
            {
                return true;
            }

            if (!services.TryGetOptionalService<TElementImplementation>(out var instance))
            {
                return false;
            }

            services.AddToCollection<TElementInterface>(instance!);
            return true;
        });
    }

    public ServiceProvider Build()
    {
        var serviceProvider = new ServiceProvider();
        var pending         = new Queue<Func<ServiceProvider, bool>>();
        var unresolved      = new Queue<Func<ServiceProvider, bool>>();

        foreach (var type in m_implementations)
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
                if (parameterTypes.Any(x => !m_interfaces.Contains(x)))
                {
                    continue;
                }

                selectedConstructor = constructor;
                selectedParameters  = parameterTypes;
            }

            pending.Enqueue(services =>
            {
                if (selectedConstructor == null)
                {
                    throw new Exception($"Couldn't find suitable constructor for type {type.Name}.");
                }

                var parameters = selectedParameters.Select(services.GetService).ToArray();
                if (parameters.Any(x => x == null))
                {
                    return false;
                }

                var instance = selectedConstructor.Invoke(parameters);
                services.AddSingleton(type, instance);

                if (instance is IDisposable disposable)
                {
                    services.AddDestructor(disposable);
                }

                return true;
            });
        }

        foreach (var step in m_steps)
        {
            pending.Enqueue(step);
        }

        while (pending.Count != 0)
        {
            var beforeCount = pending.Count;
            while (pending.TryDequeue(out var work))
            {
                if (!work(serviceProvider))
                {
                    unresolved.Enqueue(work);
                }
            }

            if (beforeCount == unresolved.Count)
            {
                throw new Exception("Circular dependencies while resolving services.");
            }

            while (unresolved.TryDequeue(out var work))
            {
                pending.Enqueue(work);
            }
        }

        return serviceProvider;
    }
}