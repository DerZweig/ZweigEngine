namespace ZweigEngine.Common.Services;

internal class ServiceProviderProxy : IServiceProvider
{
    private readonly IReadOnlyDictionary<Type, object> m_services;

    public ServiceProviderProxy(IReadOnlyDictionary<Type, object> services)
    {
        m_services = services;
    }

    public object? GetService(Type serviceType)
    {
        if (serviceType == typeof(IServiceProvider))
        {
            return this;
        }
        
        return m_services.GetValueOrDefault(serviceType);
    }
}