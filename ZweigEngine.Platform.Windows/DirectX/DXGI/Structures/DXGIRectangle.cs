using System.Runtime.InteropServices;

namespace ZweigEngine.Platform.Windows.DirectX.DXGI.Structures;

[StructLayout(LayoutKind.Sequential)]
internal struct DXGIRectangle
{
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;
}