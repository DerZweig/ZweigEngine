using ZweigEngine.Common.Services;
using ZweigEngine.Common.Services.Messages;
using ZweigEngine.Common.Services.Platform;

namespace ZweigEngine.Common;

public static class CommonServiceConfiguration
{
    public static void AddCommonServices(this ServiceConfiguration configuration)
    {
        configuration.AddSingleton<INativeLibraryLoader, NativeLibraryLoader>();
        configuration.AddSingleton<IMessageBus, MessageBus>();
    }
}