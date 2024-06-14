using ZweigEngine.Application.Constants;
using ZweigEngine.Common;
using ZweigEngine.Common.Platform.Interfaces;
using ZweigEngine.Common.Video.Interfaces;

namespace ZweigEngine.Application.Controller;

public sealed class VideoController : IDisposable
{
    private readonly IPlatformWindow              m_window;
    private readonly IVarying<IVideoOutput>  m_output;
    private readonly IVarying<IVideoBackend> m_backend;

    public VideoController(IPlatformWindow window, IVarying<IVideoOutput> output, IVarying<IVideoBackend> backend)
    {
        m_window  = window;
        m_output  = output;
        m_backend = backend;

        m_window.OnCreated += HandleWindowCreated;
        m_window.OnUpdate  += HandleWindowUpdate;
        m_window.OnClosing += HandleWindowClosing;
    }


    private void ReleaseUnmanagedResources()
    {
        m_window.OnCreated -= HandleWindowCreated;
        m_window.OnUpdate  -= HandleWindowUpdate;
        m_window.OnClosing -= HandleWindowClosing;
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~VideoController()
    {
        ReleaseUnmanagedResources();
    }

    private void HandleWindowCreated(IPlatformWindow window)
    {
        if (!m_output.EnumerateOptions().Contains(VideoSettings.RendererNameDefault) ||
            !m_backend.EnumerateOptions().Contains(VideoSettings.RendererNameDefault))
        {
            return;
        }
        
        m_output.Activate(VideoSettings.RendererNameDefault);
        m_backend.Activate(VideoSettings.RendererNameDefault);
    }

    private void HandleWindowClosing(IPlatformWindow window)
    {
        m_backend.Deactivate();
        m_output.Deactivate();
    }

    private void HandleWindowUpdate(IPlatformWindow window)
    {
        var width  = window.GetViewportWidth();
        var height = window.GetViewportHeight();

        m_output.Current?.Resize(width, height);
        //do rendering
        m_output.Current?.Present();
    }
}