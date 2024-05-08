using System.Runtime.InteropServices;
using ZweigEngine.Win32.DirectX.DXGI.Constants;

namespace ZweigEngine.Win32.DirectX.DXGI.Structures;

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