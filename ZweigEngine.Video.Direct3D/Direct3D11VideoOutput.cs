using ZweigEngine.Common.Platform;
using ZweigEngine.Common.Platform.Interfaces;
using ZweigEngine.Common.Video.Interfaces;
using ZweigEngine.Video.Direct3D.Imports.Constants;
using ZweigEngine.Video.Direct3D.Imports.D3D11;
using ZweigEngine.Video.Direct3D.Imports.DXGI;
using ZweigEngine.Video.Direct3D.Imports.DXGI.Constants;
using ZweigEngine.Video.Direct3D.Imports.DXGI.Structures;
using ZweigEngine.Video.Direct3D.Imports.Structures;
using ZweigEngine.Video.Direct3D.Imports.VTables.D3D11;

namespace ZweigEngine.Video.Direct3D;

public sealed class Direct3D11VideoOutput : IDisposable, IVideoOutput
{
    private readonly DXGIFactory            m_factory;
    private readonly D3D11Device            m_device;
    private readonly D3D11DeviceContext     m_context;
    private readonly DXGISwapChain          m_swapChain;
    private          D3D11Texture2D?        m_swapChainTexture;
    private          D3D11RenderTargetView? m_swapChainTarget;

    public Direct3D11VideoOutput(PlatformLibraryLoader loader, IPlatformWindow window)
    {
        if (!DXGIFactory.TryCreate(loader, out m_factory))
        {
            throw new Exception("Couldn't initialize DXGI Factory.");
        }

        if (!D3D11Device.TryCreate(loader, null, Direct3DDriverType.Hardware, out m_device, out m_context))
        {
            throw new Exception("Couldn't initialize D3D11 device");
        }

        var swapChainDescription = new DXGISwapChainDescription
                                   {
                                       BufferDescription = new DXGIModeDescription
                                                           {
                                                               RefreshRate = new DXGIRational
                                                                             {
                                                                                 Numerator   = 0,
                                                                                 Denominator = 1
                                                                             },
                                                               Format           = DXGIFormat.R8G8B8A8Unorm,
                                                               ScanlineOrdering = DXGIModeScanlineOrder.Unspecified,
                                                               Scaling          = DXGIModeScaling.Unspecified
                                                           },
                                       SampleDescription = new DXGISampleDescription
                                                           {
                                                               Count   = 1,
                                                               Quality = 0
                                                           },
                                       BufferUsage  = DXGIUsage.RenderTargetOutput,
                                       BufferCount  = 3,
                                       OutputWindow = window.GetNativePointer(),
                                       Windowed     = Win32Bool.True,
                                       SwapEffect   = DXGISwapEffect.Discard,
                                       Flags        = 0
                                   };

        if (!m_factory.TryMakeWindowAssociation(window.GetNativePointer(), DXGIMakeWindowAssociationFlags.NoAltEnter))
        {
            throw new Exception("Couldn't configure window for DXGI factory.");
        }

        if (!m_factory.CreateSwapChain(m_device.Self, swapChainDescription, out m_swapChain))
        {
            throw new Exception("Couldn't initialize DXGI swap chain.");
        }

        CreateSwapChainTarget();
    }

    internal int                    Width        { get; private set; }
    internal int                    Height       { get; private set; }
    internal D3D11Device            Device       => m_device;
    internal D3D11DeviceContext     Context      => m_context;
    internal D3D11RenderTargetView? RenderTarget => m_swapChainTarget;

    public void Resize(int width, int height)
    {
        if (Width == width && Height == height)
        {
            return;
        }
        
        m_context.Flush();
        m_context.ClearState();
        m_swapChainTexture?.Dispose();
        m_swapChainTarget?.Dispose();

        if (!m_swapChain.TryResizeBuffers(width, height, DXGIFormat.Unknown))
        {
            throw new Exception("Couldn't resize DXGI swap chain.");
        }

        CreateSwapChainTarget();

        Width  = width;
        Height = height;
    }

    public void Present()
    {
        if (!m_swapChain.TryPresent(1, DXGIPresentFlags.None))
        {
            throw new Exception("Couldn't present DXGI swap chain.");
        }
    }

    private void CreateSwapChainTarget()
    {
        if (!m_swapChain.TryGetBuffer(0, typeof(D3D11Texture2DMethodTable).GUID, ref m_swapChainTexture))
        {
            throw new Exception("Couldn't acquire DXGI swap chain buffer.");
        }

        if (!m_device.CreateRenderTargetView(m_swapChainTexture!, null, ref m_swapChainTarget))
        {
            throw new Exception("Couldn't create render target from DXGI swap chain buffer.");
        }
    }

    private void ReleaseUnmanagedResources()
    {
        m_context.Flush();
        m_context.ClearState();
        m_swapChain.Dispose();
        m_context.Dispose();
        m_device.Dispose();
        m_factory.Dispose();
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~Direct3D11VideoOutput()
    {
        ReleaseUnmanagedResources();
    }
}