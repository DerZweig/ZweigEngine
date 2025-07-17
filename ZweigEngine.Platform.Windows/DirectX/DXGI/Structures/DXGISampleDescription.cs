using System.Runtime.InteropServices;

namespace ZweigEngine.Platform.Windows.DirectX.DXGI.Structures;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct DXGISampleDescription
{
	public uint Count;
	public uint Quality;
}