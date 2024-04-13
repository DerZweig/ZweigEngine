using System.Runtime.InteropServices;
using ZweigEngine.Common.Core;
using ZweigEngine.Common.Interfaces;
using ZweigEngine.Common.Interfaces.Platform;
using ZweigEngine.Common.Interfaces.Video;
using ZweigEngine.OpenGL.Win32.Constants;
using ZweigEngine.OpenGL.Win32.Prototypes;
using ZweigEngine.OpenGL.Win32.Structures;

namespace ZweigEngine.OpenGL.Win32;

public class Win32OpenGLSurface : IDisposable, IOpenGLLoader, IVideoSurface
{
    private const byte                                PIXEL_FORMAT_COLOR_BITS             = 32;
    private const byte                                PIXEL_FORMAT_DEPTH_BITS             = 24;
    private const byte                                PIXEL_FORMAT_STENCIL_BITS           = 8;
    private const Win32PixelFormatDescriptorFlags     PIXEL_FORMAT_FLAGS                  = Win32PixelFormatDescriptorFlags.DrawToWindow | Win32PixelFormatDescriptorFlags.SupportOpenGL | Win32PixelFormatDescriptorFlags.Doublebuffer;
    private const Win32PixelFormatDescriptorPixelType PIXEL_FORMAT_TYPE                   = Win32PixelFormatDescriptorPixelType.TypeRgba;
    private const int                                 WGL_CONTEXT_ATTRIBUTE_FLAGS         = 0x2094;
    private const int                                 WGL_CONTEXT_ATTRIBUTE_MAJOR_VERSION = 0x2091;
    private const int                                 WGL_CONTEXT_ATTRIBUTE_MINOR_VERSION = 0x2092;
    private const int                                 WGL_CONTEXT_ATTRIBUTE_PROFILE_MASK  = 0x9126;
    private const int                                 WGL_CONTEXT_CORE_PROFILE_BIT        = 0x00000001;
    private const int                                 WGL_CONTEXT_FORWARD_COMPATIBLE_BIT  = 0x00000002;

    // ReSharper disable InconsistentNaming
    private readonly PfnGetDeviceContextDelegate     GetDeviceContext;
    private readonly PfnReleaseDeviceContextDelegate ReleaseDeviceContext;
    private readonly PfnSetPixelFormatDelegate       SetPixelFormat;
    private readonly PfnChoosePixelFormatDelegate    ChoosePixelFormat;
    private readonly PfnSwapBuffersDelegate          SwapBuffers;
    private readonly PfnCreateContextDelegate        WglCreateContext;
    private readonly PfnDeleteContextDelegate        WglDeleteContext;
    private readonly PfnGetProcAddressDelegate       WglGetProcAddress;
    private readonly PfnMakeCurrentDelegate          WglMakeCurrent;
    // ReSharper restore InconsistentNaming

    private readonly NativeLibraryLoader         m_loader;
    private readonly IPlatformWindow             m_window;
    private readonly Dictionary<string, object?> m_functions;
    private readonly int                         m_version_major;
    private readonly int                         m_version_minor;
    private          int                         m_width;
    private          int                         m_height;
    private          IntPtr                      m_device_context;
    private          IntPtr                      m_graphics_context;
    private          IntPtr                      m_dummy_context;


    public Win32OpenGLSurface(NativeLibraryLoader libraryLoader, IPlatformWindow window, int majorVersion, int minorVersion)
    {
        m_version_major = majorVersion;
        m_version_minor = minorVersion;
        var pixelFormatDescriptor = new Win32PixelFormatDescriptor
                                    {
                                        nSize        = (ushort)Marshal.SizeOf(typeof(Win32PixelFormatDescriptor)),
                                        nVersion     = 1,
                                        dwFlags      = PIXEL_FORMAT_FLAGS,
                                        iPixelType   = PIXEL_FORMAT_TYPE,
                                        cColorBits   = PIXEL_FORMAT_COLOR_BITS,
                                        cDepthBits   = PIXEL_FORMAT_DEPTH_BITS,
                                        cStencilBits = PIXEL_FORMAT_STENCIL_BITS,
                                        iLayerType   = Win32PixelFormatDescriptorLayerTypes.MainPlane
                                    };

        var contextAttributes = new[]
                                {
                                    WGL_CONTEXT_ATTRIBUTE_MAJOR_VERSION, majorVersion,
                                    WGL_CONTEXT_ATTRIBUTE_MINOR_VERSION, minorVersion,
                                    WGL_CONTEXT_ATTRIBUTE_FLAGS, WGL_CONTEXT_FORWARD_COMPATIBLE_BIT,
                                    WGL_CONTEXT_ATTRIBUTE_PROFILE_MASK, WGL_CONTEXT_CORE_PROFILE_BIT,
                                    0
                                };

        libraryLoader.LoadFunction("user32", "GetDC", out GetDeviceContext);
        libraryLoader.LoadFunction("user32", "ReleaseDC", out ReleaseDeviceContext);
        libraryLoader.LoadFunction("gdi32", "ChoosePixelFormat", out ChoosePixelFormat);
        libraryLoader.LoadFunction("gdi32", "SetPixelFormat", out SetPixelFormat);
        libraryLoader.LoadFunction("gdi32", "SwapBuffers", out SwapBuffers);
        libraryLoader.LoadFunction("opengl32", "wglCreateContext", out WglCreateContext);
        libraryLoader.LoadFunction("opengl32", "wglDeleteContext", out WglDeleteContext);
        libraryLoader.LoadFunction("opengl32", "wglMakeCurrent", out WglMakeCurrent);
        libraryLoader.LoadFunction("opengl32", "wglGetProcAddress", out WglGetProcAddress);

        m_loader    = libraryLoader;
        m_window    = window;
        m_functions = new Dictionary<string, object?>();

        m_device_context = GetDeviceContext(m_window.GetNativePointer());
        if (m_device_context == IntPtr.Zero)
        {
            throw new Exception("Couldn't retrieve device context from window.");
        }

        var pixelFormatIdentifier = ChoosePixelFormat(m_device_context, ref pixelFormatDescriptor);
        if (pixelFormatIdentifier == 0 || !SetPixelFormat(m_device_context, pixelFormatIdentifier, ref pixelFormatDescriptor))
        {
            throw new Exception("Couldn't configure suitable device pixel format.");
        }

        m_dummy_context = WglCreateContext(m_device_context);
        if (m_dummy_context == IntPtr.Zero || !WglMakeCurrent(m_device_context, m_dummy_context))
        {
            throw new Exception("Couldn't setup generic opengl context.");
        }

        LoadFunction("wglCreateContextAttribsARB", out PfnWglCreateContextAttribsArb wglCreateContextAttribsArb);

        m_graphics_context = wglCreateContextAttribsArb(m_device_context, IntPtr.Zero, contextAttributes);
        if (m_graphics_context == IntPtr.Zero || !WglMakeCurrent(m_device_context, m_graphics_context))
        {
            throw new Exception("Couldn't setup opengl core context.");
        }

        WglDeleteContext(m_dummy_context);
        m_dummy_context = IntPtr.Zero;
    }

    private void ReleaseUnmanagedResources()
    {
        if (m_dummy_context != IntPtr.Zero || m_graphics_context != IntPtr.Zero)
        {
            WglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
            if (m_dummy_context != IntPtr.Zero)
            {
                WglDeleteContext(m_dummy_context);
                m_dummy_context = IntPtr.Zero;
            }

            if (m_graphics_context != IntPtr.Zero)
            {
                WglDeleteContext(m_graphics_context);
                m_graphics_context = IntPtr.Zero;
            }
        }

        if (m_device_context != IntPtr.Zero)
        {
            ReleaseDeviceContext(m_window.GetNativePointer(), m_device_context);
            m_device_context = IntPtr.Zero;
        }
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~Win32OpenGLSurface()
    {
        ReleaseUnmanagedResources();
    }

    public int GetVersionMajor() => m_version_major;
    public int GetVersionMinor() => m_version_minor;

    public int GetWidth() => m_width;
    public int GetHeight() => m_height;

    public void Resize(int width, int height)
    {
        m_width  = width;
        m_height = height;
    }

    public void Present()
    {
        SwapBuffers(m_device_context);
    }

    public void LoadFunction<TDelegate>(string exportName, out TDelegate func) where TDelegate : Delegate
    {
        if (!TryLoadFunction<TDelegate>(exportName, out var temp))
        {
            throw new Exception($"Couldn't load required function {exportName}.");
        }

        func = temp;
    }

    public bool TryLoadFunction<TDelegate>(string exportName, out TDelegate func) where TDelegate : Delegate
    {
        if (m_functions.TryGetValue(exportName, out var cached))
        {
            func = (TDelegate?)cached!;
            return cached != null;
        }

        var address = WglGetProcAddress(exportName);
        if (address != IntPtr.Zero)
        {
            func = Marshal.GetDelegateForFunctionPointer<TDelegate>(address);

            m_functions[exportName] = func;
            return true;
        }
        else if (m_loader.TryLoadFunction("opengl32", exportName, out func))
        {
            m_functions[exportName] = func;
            return true;
        }

        return false;
    }
}