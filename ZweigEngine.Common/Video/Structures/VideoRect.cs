using System.Runtime.InteropServices;

namespace ZweigEngine.Common.Video.Structures;

[StructLayout(LayoutKind.Sequential)]
public struct VideoRect
{
    public int Left;
    public int Top;
    public int Width;
    public int Height;
}