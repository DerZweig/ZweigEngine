using System.Runtime.InteropServices;
using ZweigEngine.Common;
using ZweigEngine.Common.Services;
using ZweigEngine.Platform.Windows;

namespace ZweigEngine.Application;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        var config = new ServiceConfiguration();
        config.AddCommonServices();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            config.AddWindowsPlatformServices();
        }

        using (var host = ServiceHost.Create(config))
        {
            host.InitializeServices();
            
        }
    }
}