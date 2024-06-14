using System.Runtime.InteropServices;
using ZweigEngine.Application.Constants;
using ZweigEngine.Application.Controller;
using ZweigEngine.Common;
using ZweigEngine.Common.Platform;
using ZweigEngine.Common.Platform.Interfaces;
using ZweigEngine.Common.Utility.Extensions;
using ZweigEngine.Common.Video.Interfaces;
using ZweigEngine.Video.Direct3D;
using ZweigEngine.Video.OpenGL;
using ZweigEngine.Video.OpenGL.Win32;
using ZweigEngine.Win32;

namespace ZweigEngine.Application;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        var serviceConfig = new ServiceConfiguration();
        serviceConfig.AddSingleton<PlatformLibraryLoader>();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            serviceConfig.AddSingleton<IWin32DPIScalingHandler, IWin32DPIScalingHandler.ProcessPerMonitor>();

            serviceConfig.AddSingleton<IPlatformWindow, Win32Window>();
            serviceConfig.AddSingleton<IPlatformKeyboard, Win32Keyboard>();
            serviceConfig.AddSingleton<IPlatformMouse, Win32Mouse>();
            serviceConfig.AddSingleton<IPlatformDisplayInfo, Win32PlatformDisplayInfo>();
            serviceConfig.AddVarying<IVideoBackend, Direct3D11VideoBackend>(VideoSettings.RendererNameDirect3D);
            serviceConfig.AddVarying<IVideoOutput, Direct3D11VideoOutput>(VideoSettings.RendererNameDirect3D);
            serviceConfig.AddVarying<IVideoBackend, OpenGLBackend>(VideoSettings.RendererNameOpenGL);
            serviceConfig.AddVarying<IVideoOutput, Win32OpenGLOutput>(VideoSettings.RendererNameOpenGL);
        }

        serviceConfig.AddSingleton<VideoController>();
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
}