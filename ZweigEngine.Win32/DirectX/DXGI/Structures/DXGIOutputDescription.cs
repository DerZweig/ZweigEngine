using System.Runtime.InteropServices;
using ZweigEngine.Win32.DirectX.DXGI.Constants;
using ZweigEngine.Win32.Structures;

namespace ZweigEngine.Win32.DirectX.DXGI.Structures;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct DXGIOutputDescription
{
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string DeviceName;

	public DXGIRectangle    DesktopCoordinates;
	public Win32Bool        AttachedToDesktop;
	public DXGIModeRotation ModeRotation;
	public IntPtr           MonitorHandle;
}