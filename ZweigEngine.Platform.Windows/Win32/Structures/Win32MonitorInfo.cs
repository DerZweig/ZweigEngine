using System.Runtime.InteropServices;

namespace ZweigEngine.Platform.Windows.Win32.Structures;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct Win32MonitorInfo
{
    public int            Size;
    public Win32Rectangle MonitorRect;
    public Win32Rectangle WorkRect;
    public int            Flags;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string Name;
}