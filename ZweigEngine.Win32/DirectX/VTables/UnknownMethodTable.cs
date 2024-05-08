using System.Runtime.InteropServices;

namespace ZweigEngine.Win32.DirectX.VTables;

[Guid("00000000-0000-0000-C000-000000000046")]
[StructLayout(LayoutKind.Sequential)]
internal struct UnknownMethodTable
{
	public IntPtr QueryInterface;
	public IntPtr AddRef;
	public IntPtr Release;
}