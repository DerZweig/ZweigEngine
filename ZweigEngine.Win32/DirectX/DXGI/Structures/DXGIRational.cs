using System.Runtime.InteropServices;

namespace ZweigEngine.Win32.DirectX.DXGI.Structures;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct DXGIRational
{
	public uint Numerator;
	public uint Denominator;
}