using ZweigEngine.Common.Services.Platform;
using ZweigEngine.Platform.Windows.Win32.Constants;
using ZweigEngine.Platform.Windows.Win32.Structures;

namespace ZweigEngine.Platform.Windows.Win32.Prototypes;


internal class ShCore
{
    public ShCore(INativeLibraryLoader loader)
    {
        loader.LoadFunction("shcore", nameof(SetProcessDpiAwareness), out SetProcessDpiAwareness);
    }
    
    public readonly PfnSetProcessDpiAwarenessDelegate SetProcessDpiAwareness;
    
    public delegate Win32HResult PfnSetProcessDpiAwarenessDelegate(Win32ProcessDpiAwareness value);
}