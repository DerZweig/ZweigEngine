using ZweigEngine.Common.Platform;
using ZweigEngine.Win32.Constants;
using ZweigEngine.Win32.Prototypes;

namespace ZweigEngine.Win32;

public interface IWin32DPIScalingHandler
{
    public class Ignore : IWin32DPIScalingHandler
    {
    }

    public class ProcessPerMonitor : IWin32DPIScalingHandler
    {
        public ProcessPerMonitor(PlatformLibraryLoader libraryLoader)
        {
            if (libraryLoader.TryLoadFunction<PfnSetProcessDpiAwarenessDelegate>("Shcore", "SetProcessDpiAwareness", out var func))
            {
                func(Win32ProcessDpiAwareness.PerMonitorDpiAware);
            }
        }
    }
}

