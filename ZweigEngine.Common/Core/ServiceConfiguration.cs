﻿using System.Reflection;

namespace ZweigEngine.Common.Core;

public sealed class ServiceConfiguration
{
    private readonly List<Entry> m_entries;

    public ServiceConfiguration()
    {
        m_entries = new List<Entry>();
    }

    public void AddSingleton<TInterface, TImplementation>(Action<TImplementation> configure) where TImplementation : TInterface
    {
        m_entries.Add(new Entry
                      {
                          InterfaceType      = typeof(TInterface),
                          ImplementationType = typeof(TImplementation),
                          Configure          = obj => configure((TImplementation)obj)
                      });
    }

    public void AddSingleton<TInterface, TImplementation>() where TImplementation : TInterface
    {
        m_entries.Add(new Entry
                      {
                          InterfaceType      = typeof(TInterface),
                          ImplementationType = typeof(TImplementation),
                          Configure          = null
                      });
    }

    public void AddSingleton<TImplementation>(Action<TImplementation> configure)
    {
        AddSingleton<TImplementation, TImplementation>(configure);
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

        foreach (var entry in m_entries)
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

        foreach (var entry in m_entries)
        {
            var type = entry.ImplementationType;
            var constructors = type.GetConstructors().Select(ctor => new ServiceDescriptor
                                                                     {
                                                                         InterfaceType      = entry.InterfaceType,
                                                                         ImplementationType = entry.ImplementationType,
                                                                         Configure          = entry.Configure,
                                                                         Constructor        = ctor,
                                                                         ParameterTypes     = ctor.GetParameters().Select(param => param.ParameterType).ToArray()
                                                                     }).ToArray();

            var sorted = constructors.OrderByDescending(x => x.ParameterTypes.Length);
            var best   = sorted.FirstOrDefault();

            if (best == null || best.ParameterTypes.Any(p => !interfaces.Contains(p)))
            {
                throw new InvalidOperationException($"Couldn't find suitable constructor for service {type.Name}");
            }

            pending.Enqueue(best);
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
                    descriptor.Configure?.Invoke(instance);

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
        public Type            InterfaceType      { get; init; } = null!;
        public Type            ImplementationType { get; init; } = null!;
        public Action<object>? Configure          { get; init; }
    }

    private class ServiceDescriptor : Entry
    {
        public ConstructorInfo Constructor        { get; init; } = null!;
        public Type[]          ParameterTypes     { get; init; } = null!;
    }
}