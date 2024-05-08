using System.Runtime.InteropServices;

namespace ZweigEngine.Win32.DirectX.VTables.DXGI;

[Guid("2411e7e1-12ac-4ccf-bd14-9798e8534dc0")]
[StructLayout(LayoutKind.Sequential)]
internal struct DXGIAdapterMethodTable
{
	public DXGIObjectMethodTable Super;
	public IntPtr                EnumOutputs;
	public IntPtr                GetDesc;
	public IntPtr                CheckInterfaceSupport;
}