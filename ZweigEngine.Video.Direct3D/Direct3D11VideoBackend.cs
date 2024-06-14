using ZweigEngine.Common;
using ZweigEngine.Common.Video.Interfaces;

namespace ZweigEngine.Video.Direct3D;

public sealed class Direct3D11VideoBackend : IDisposable, IVideoBackend
{
    private readonly Direct3D11VideoOutput m_output;

    public Direct3D11VideoBackend(IVarying<IVideoOutput> output)
    {
        m_output = (Direct3D11VideoOutput)output.Current!;
    }

    private void ReleaseUnmanagedResources()
    {
        // TODO release unmanaged resources here
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~Direct3D11VideoBackend()
    {
        ReleaseUnmanagedResources();
    }
}