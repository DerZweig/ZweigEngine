using System.Runtime.InteropServices;
using ZweigEngine.Common.Services.Platform;

namespace ZweigEngine.Platform.Windows.OpenGL.Libraries;

public class OpenGL32
{
    public OpenGL32(INativeLibraryLoader loader)
    {
        loader.LoadFunction("opengl32", "wglCreateContext", out CreateContext);
        loader.LoadFunction("opengl32", "wglDeleteContext", out DeleteContext);
        loader.LoadFunction("opengl32", "wglMakeCurrent", out MakeCurrent);
        loader.LoadFunction("opengl32", "wglGetCurrent", out GetCurrent);
        loader.LoadFunction("opengl32", "wglGetProcAddress", out GetProcAddress);
    }

    public readonly PfnCreateContextDelegate     CreateContext;
    public readonly PfnDeleteContextDelegate     DeleteContext;
    public readonly PfnGetProcAddressDelegate    GetProcAddress;
    public readonly PfnMakeCurrentDelegate       MakeCurrent;
    public readonly PfnGetCurrentContextDelegate GetCurrent;

    public delegate IntPtr PfnCreateContextDelegate(IntPtr deviceContext);

    public delegate bool PfnDeleteContextDelegate(IntPtr renderContext);

    public delegate IntPtr PfnGetCurrentContextDelegate();

    public delegate IntPtr PfnGetProcAddressDelegate([MarshalAs(UnmanagedType.LPStr)] string name);

    public delegate bool PfnMakeCurrentDelegate(IntPtr deviceContext, IntPtr renderContext);

    //wglCreateContextAttribsARB must be loaded after context is created 
    //internal delegate IntPtr PfnWglCreateContextAttribsArb(IntPtr deviceContext, IntPtr openglContext, int[] attributes);
}