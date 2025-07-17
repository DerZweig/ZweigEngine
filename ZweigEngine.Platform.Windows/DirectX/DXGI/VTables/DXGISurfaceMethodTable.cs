using System.Runtime.InteropServices;

namespace ZweigEngine.Platform.Windows.DirectX.DXGI.VTables;

[Guid("cafcb56c-6ac3-4889-bf47-9e23bbd260ec")]
[StructLayout(LayoutKind.Sequential)]
internal struct DXGISurfaceMethodTable
{
	public DXGIDeviceSubObjectMethodTable Super;
	public IntPtr                         GetDesc;
	public IntPtr                         Map;
	public IntPtr                         Unmap;
}