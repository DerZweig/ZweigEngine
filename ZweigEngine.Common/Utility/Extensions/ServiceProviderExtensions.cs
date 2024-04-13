﻿namespace ZweigEngine.Common.Utility.Extensions;

public static class ServiceProviderExtensions
{
    public static bool TryGetOptionalService<TService>(this IServiceProvider services, out TService service)
    {
        var temp = services.GetService(typeof(TService));
        if (temp != null)
        {
            service = (TService)temp;
            return true;
        }
        else
        {
            service = default!; //throw null reference if something attempts to access the result
            return false;
        }
    }
    
    public static TService GetRequiredService<TService>(this IServiceProvider services)
    {
        var service = services.GetService(typeof(TService)) ?? throw new NullReferenceException();
        return (TService)service;
    }
}