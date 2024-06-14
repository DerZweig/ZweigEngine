using System.Runtime.InteropServices;

namespace ZweigEngine.Video.Direct3D.Imports.VTables.DXGI;

[Guid("cafcb56c-6ac3-4889-bf47-9e23bbd260ec")]
[StructLayout(LayoutKind.Sequential)]
internal struct DXGISurfaceMethodTable
{
	public DXGIDeviceSubObjectMethodTable Super;
	public IntPtr                         GetDesc;
	public IntPtr                         Map;
	public IntPtr                         Unmap;
}