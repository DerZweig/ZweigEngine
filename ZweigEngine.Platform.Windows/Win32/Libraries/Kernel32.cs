using ZweigEngine.Common.Services.Platform;

namespace ZweigEngine.Platform.Windows.Win32.Libraries;


internal class Kernel32
{
    public Kernel32(INativeLibraryLoader loader)
    {
        loader.LoadFunction("kernel32", "GetModuleHandleW", out GetModuleHandle);
    }
    
    public readonly PfnGetModuleHandleDelegate GetModuleHandle;
    
    public delegate IntPtr PfnGetModuleHandleDelegate(IntPtr modulePointer);

}