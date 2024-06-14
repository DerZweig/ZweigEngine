using System.Runtime.InteropServices;
using ZweigEngine.Video.Direct3D.Imports.DXGI.Constants;

namespace ZweigEngine.Video.Direct3D.Imports.DXGI.Structures;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct DXGIModeDescription
{
	public uint                  Width;
	public uint                  Height;
	public DXGIRational          RefreshRate;
	public DXGIFormat            Format;
	public DXGIModeScanlineOrder ScanlineOrdering;
	public DXGIModeScaling       Scaling;
}