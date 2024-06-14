using System.Runtime.InteropServices;

namespace ZweigEngine.Video.Direct3D.Imports.DXGI.Structures;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct DXGIRational
{
	public uint Numerator;
	public uint Denominator;
}