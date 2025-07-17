using System.Runtime.InteropServices;
using ZweigEngine.Common;
using ZweigEngine.Common.Assets;
using ZweigEngine.Common.Platform;
using ZweigEngine.Common.Utility.Extensions;
using ZweigEngine.Common.Utility.Interop;
using ZweigEngine.Image.DDS;
using ZweigEngine.Image.TGA;
using ZweigEngine.Platform.Windows.Win32;

namespace ZweigEngine.Application;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        var config = new ServiceConfiguration();
        config.AddSingleton<NativeLibraryLoader>();
        config.AddVariant<IImageReader, DDSImageReader>();
        config.AddVariant<IImageReader, TGAImageReader>();
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            config.AddSingleton<IPlatformWindow, Win32Window>();
            config.AddSingleton<IPlatformKeyboard, Win32Keyboard>();
            config.AddSingleton<IPlatformMouse, Win32Mouse>();
        }
        
        using (var services = config.Build())
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
}