using System.Runtime.InteropServices;

namespace ZweigEngine.Video.Direct3D.Imports.D3D11.Structures;

[StructLayout(LayoutKind.Sequential)]
public struct D3D11Viewport
{
	public float TopLeftX;
	public float TopLeftY;
	public float Width;
	public float Height;
	public float MinDepth;
	public float MaxDepth;
}