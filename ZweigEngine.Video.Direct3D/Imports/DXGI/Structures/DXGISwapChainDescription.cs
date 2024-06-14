using System.Runtime.InteropServices;
using ZweigEngine.Video.Direct3D.Imports.DXGI.Constants;
using ZweigEngine.Video.Direct3D.Imports.Structures;

namespace ZweigEngine.Video.Direct3D.Imports.DXGI.Structures;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct DXGISwapChainDescription
{
	public DXGIModeDescription   BufferDescription;
	public DXGISampleDescription SampleDescription;
	public DXGIUsage             BufferUsage;
	public uint                  BufferCount;
	public IntPtr                OutputWindow;
	public Win32Bool             Windowed;
	public DXGISwapEffect        SwapEffect;
	public uint                  Flags;
}