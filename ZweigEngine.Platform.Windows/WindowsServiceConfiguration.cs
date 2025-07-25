using ZweigEngine.Common.Services;
using ZweigEngine.Platform.Windows.DirectX.DXGI;
using ZweigEngine.Platform.Windows.Win32;

namespace ZweigEngine.Platform.Windows;

public static class WindowsServiceConfiguration
{
    public static void AddWindowsPlatformServices(this ServiceConfiguration configuration)
    {
        configuration.AddSingleton<Win32Synchronization>();
        configuration.AddSingleton<DXGI>();
    }
}