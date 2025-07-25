using ZweigEngine.Common.Services.Platform;
using ZweigEngine.Platform.Windows.Win32.Structures;

namespace ZweigEngine.Platform.Windows.Win32.Prototypes;

internal class Gdi32
{
    public Gdi32(INativeLibraryLoader loader)
    {
        loader.LoadFunction("gdi32", "ChoosePixelFormat", out ChoosePixelFormat);
        loader.LoadFunction("gdi32", "SetPixelFormat", out SetPixelFormat);
        loader.LoadFunction("gdi32", "SwapBuffers", out SwapBuffers);
    }

    public readonly PfnSetPixelFormatDelegate    SetPixelFormat;
    public readonly PfnChoosePixelFormatDelegate ChoosePixelFormat;
    public readonly PfnSwapBuffersDelegate       SwapBuffers;

    public delegate int PfnChoosePixelFormatDelegate(IntPtr deviceContext, ref Win32PixelFormatDescriptor pixelFormatDescriptor);

    public delegate bool PfnSetPixelFormatDelegate(IntPtr deviceContext, int pixelFormat, ref Win32PixelFormatDescriptor pixelFormatDescriptor);

    public delegate void PfnSwapBuffersDelegate(IntPtr deviceContext);
}