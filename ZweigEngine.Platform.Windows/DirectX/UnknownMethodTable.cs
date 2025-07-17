using System.Runtime.InteropServices;

namespace ZweigEngine.Platform.Windows.DirectX;

[Guid("00000000-0000-0000-C000-000000000046")]
[StructLayout(LayoutKind.Sequential)]
internal struct UnknownMethodTable
{
    public IntPtr QueryInterface;
    public IntPtr AddRef;
    public IntPtr Release;
}