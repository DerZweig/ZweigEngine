using System.Diagnostics;
using System.Runtime.InteropServices;
using ZweigEngine.Application.Services.Video;
using ZweigEngine.Common.Core;
using ZweigEngine.Common.Platform.Interfaces;
using ZweigEngine.Common.Utility.Extensions;
using ZweigEngine.Win32;

namespace ZweigEngine.Application;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        try
        {
            var serviceConfig = new ServiceConfiguration();
            serviceConfig.AddSingleton<NativeLibraryLoader>();
            

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                serviceConfig.AddSingleton<IPlatformWindow, Win32Window>();
                serviceConfig.AddSingleton<IPlatformKeyboard, Win32Keyboard>();
                serviceConfig.AddSingleton<IPlatformMouse, Win32Mouse>();
                serviceConfig.AddSingleton<IPlatformInfo, Win32PlatformInfo>();
                serviceConfig.AddSingleton<Win32VideoContext>();
            }

            using (var services = serviceConfig.Build())
            {
                var window = services.GetRequiredService<IPlatformWindow>();

                window.Create();
                window.SetTitle("ZweigEngine::Demo");
                while (window.IsAvailable())
                {
                    window.Update();
                }
            }
        }
        catch (Exception ex)
        {
            if (Debugger.IsAttached)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace ?? string.Empty);
                Debugger.Break();
            }
        }
    }
}