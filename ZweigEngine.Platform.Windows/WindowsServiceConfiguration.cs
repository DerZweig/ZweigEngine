using ZweigEngine.Common.Services;
using ZweigEngine.Platform.Windows.DirectX.DXGI;
using ZweigEngine.Platform.Windows.Win32;
using ZweigEngine.Platform.Windows.Win32.Prototypes;

namespace ZweigEngine.Platform.Windows;

public static class WindowsServiceConfiguration
{
    public static void AddWindowsPlatformServices(this ServiceConfiguration configuration)
    {
        configuration.AddSingleton<Win32Synchronization>();
        configuration.AddSingleton<DXGI>();
        configuration.AddSingleton<Kernel32>();
        configuration.AddSingleton<User32>();
        configuration.AddSingleton<Gdi32>();
        configuration.AddSingleton<ShCore>();
    }
}