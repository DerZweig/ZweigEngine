using System.Reflection;

namespace ZweigEngine.Common;

public sealed class ServiceConfiguration
{
    private readonly List<Entry> m_singletons;
    private readonly List<Entry> m_varying;

    public ServiceConfiguration()
    {
        m_singletons = new List<Entry>();
        m_varying    = new List<Entry>();
    }

    public void AddVarying<TInterface, TImplementation>(string name) where TImplementation : TInterface
    {
        m_varying.Add(new Entry
                      {
                          Name               = name,
                          InterfaceType      = typeof(TInterface),
                          ImplementationType = typeof(TImplementation)
                      });
    }

    public void AddSingleton<TInterface, TImplementation>() where TImplementation : TInterface
    {
        m_singletons.Add(new Entry
                         {
                             InterfaceType      = typeof(TInterface),
                             ImplementationType = typeof(TImplementation)
                         });
    }

    public void AddSingleton<TImplementation>()
    {
        AddSingleton<TImplementation, TImplementation>();
    }

    public ServiceProvider Build()
    {
        var serviceProvider = new ServiceProvider();
        var pending         = new Queue<ServiceDescriptor>();

        var interfaces = new HashSet<Type>
                         {
                             typeof(IServiceProvider)
                         };

        foreach (var configurable in m_varying)
        {
            var type = typeof(IVarying<>).MakeGenericType(configurable.InterfaceType);
            interfaces.Add(type);
        }

        foreach (var entry in m_singletons)
        {
            if (!interfaces.Add(entry.InterfaceType))
            {
                throw new InvalidOperationException($"Duplicate service registration of type {entry.InterfaceType}");
            }

            if (entry.InterfaceType != entry.ImplementationType)
            {
                interfaces.Add(entry.ImplementationType);
            }
        }

        foreach (var configurableGroup in m_varying.GroupBy(x => x.InterfaceType))
        {
            var interfaceType     = typeof(IVarying<>).MakeGenericType(configurableGroup.Key);
            var containerType     = typeof(Varying<>).MakeGenericType(configurableGroup.Key);
            var containerInstance = Activator.CreateInstance(containerType);
            var containerMethod   = containerType.GetMethod(nameof(Varying<object>.AddOption));


            foreach (var entry in configurableGroup)
            {
                var descriptor = ServiceDescriptor.Create(entry);
                if (string.IsNullOrWhiteSpace(entry.Name))
                {
                    throw new InvalidOperationException($"Missing option name for {entry.ImplementationType.Name}");
                }

                if (descriptor == null || descriptor.ParameterTypes.Any(p => !interfaces.Contains(p)))
                {
                    throw new InvalidOperationException($"Couldn't find suitable constructor for {entry.ImplementationType.Name}");
                }

                var factory = () =>
                {
                    var parameters = descriptor.ParameterTypes.Select(x => serviceProvider.GetService(x)).ToArray();
                    var instance   = descriptor.Constructor.Invoke(parameters);
                    return instance;
                };

                containerMethod!.Invoke(containerInstance, [entry.Name, factory]);
            }

            serviceProvider.AddSingleton(interfaceType, containerInstance!);
        }

        foreach (var entry in m_singletons)
        {
            var descriptor = ServiceDescriptor.Create(entry);
            if (descriptor == null || descriptor.ParameterTypes.Any(p => !interfaces.Contains(p)))
            {
                throw new InvalidOperationException($"Couldn't find suitable constructor for {entry.ImplementationType.Name}");
            }

            pending.Enqueue(descriptor);
        }

        while (pending.Any())
        {
            var pendingCount      = pending.Count;
            var unresolvedPending = new Queue<ServiceDescriptor>();

            while (pending.TryDequeue(out var descriptor))
            {
                var parameters = descriptor.ParameterTypes.Select(x => serviceProvider.GetService(x)).ToArray();
                if (parameters.Any(x => x == null))
                {
                    unresolvedPending.Enqueue(descriptor);
                }
                else
                {
                    var instance = descriptor.Constructor.Invoke(parameters);

                    serviceProvider.AddSingleton(descriptor.InterfaceType, instance);
                    if (descriptor.InterfaceType != descriptor.ImplementationType)
                    {
                        serviceProvider.AddSingleton(descriptor.ImplementationType, instance);
                    }

                    if (instance is IDisposable disposable)
                    {
                        serviceProvider.AddDestructor(disposable);
                    }
                }
            }

            if (unresolvedPending.Count == pendingCount)
            {
                throw new InvalidOperationException("Couldn't resolve service dependencies. Please check for circular constructor paths.");
            }

            while (unresolvedPending.TryDequeue(out var item))
            {
                pending.Enqueue(item);
            }
        }

        return serviceProvider;
    }


    private class Entry
    {
        public string? Name               { get; set; }
        public Type    InterfaceType      { get; init; } = null!;
        public Type    ImplementationType { get; init; } = null!;
    }

    private class ServiceDescriptor
    {
        public string?         Name               { get; set; }
        public Type            InterfaceType      { get; init; } = null!;
        public Type            ImplementationType { get; init; } = null!;
        public ConstructorInfo Constructor        { get; init; } = null!;
        public Type[]          ParameterTypes     { get; init; } = null!;

        public static ServiceDescriptor? Create(Entry entry)
        {
            var type = entry.ImplementationType;
            var constructors = type.GetConstructors().Select(ctor => new ServiceDescriptor
                                                                     {
                                                                         Name               = entry.Name,
                                                                         InterfaceType      = entry.InterfaceType,
                                                                         ImplementationType = entry.ImplementationType,
                                                                         Constructor        = ctor,
                                                                         ParameterTypes     = ctor.GetParameters().Select(param => param.ParameterType).ToArray()
                                                                     }).ToArray();

            var sorted = constructors.OrderByDescending(x => x.ParameterTypes.Length);
            return sorted.FirstOrDefault();
        }
    }
}