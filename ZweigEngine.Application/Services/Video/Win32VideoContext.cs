using ZweigEngine.Common.Core;
using ZweigEngine.Common.Platform.Interfaces;
using ZweigEngine.OpenGL;
using ZweigEngine.OpenGL.Win32;

namespace ZweigEngine.Application.Services.Video;

public class Win32VideoContext : IDisposable
{
    private readonly NativeLibraryLoader m_libraryLoader;
    private readonly IPlatformWindow     m_window;
    private          Win32OpenGLOutput?  m_surface;
    private          OpenGLBackend?      m_backend;

    public Win32VideoContext(NativeLibraryLoader libraryLoader, IPlatformWindow window)
    {
        m_libraryLoader = libraryLoader;
        m_window        = window;

        m_window.OnCreated += HandleWindowCreated;
        m_window.OnClosing += HandleWindowClosing;
        m_window.OnUpdate  += HandleWindowUpdate;
    }

    private void ReleaseUnmanagedResources()
    {
        m_backend?.Dispose();
        m_surface?.Dispose();
        m_backend = null;
        m_surface = null;
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~Win32VideoContext()
    {
        ReleaseUnmanagedResources();
    }

    private void HandleWindowCreated(IPlatformWindow window)
    {
        m_surface = new Win32OpenGLOutput(m_libraryLoader, m_window, 3, 3);
        m_backend = new OpenGLBackend(m_surface);
    }

    private void HandleWindowClosing(IPlatformWindow window)
    {
        ReleaseUnmanagedResources();
    }

    private void HandleWindowUpdate(IPlatformWindow window)
    {
        if (m_surface == null || m_backend == null)
        {
            return;
        }

        var width  = window.GetViewportWidth();
        var height = window.GetViewportHeight();

        m_surface.Resize(width, height);
        m_surface.Present();
    }
}