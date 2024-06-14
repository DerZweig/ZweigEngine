namespace ZweigEngine.Common.Assets.Image;

public sealed class ImageManager : IDisposable
{
    public ImageManager()
    {
        
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

    ~ImageManager()
    {
        ReleaseUnmanagedResources();
    }
}