using ZweigEngine.Common.Core;
using ZweigEngine.Common.Platform.Interfaces;
using ZweigEngine.Common.Platform.Structures;
using ZweigEngine.Win32.DirectX.DXGI;
using ZweigEngine.Win32.Structures;

namespace ZweigEngine.Win32;

public class Win32PlatformInfo : IPlatformInfo, IDisposable
{
    private readonly DXGIFactory m_factory;

    public Win32PlatformInfo(NativeLibraryLoader libraryLoader)
    {
        if (!DXGIFactory.TryCreate(libraryLoader, out m_factory))
        {
            throw new Exception("Couldn't initialize DXGI factory.");
        }
    }

    private void ReleaseUnmanagedResources()
    {
        m_factory.Dispose();
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~Win32PlatformInfo()
    {
        ReleaseUnmanagedResources();
    }

    public IReadOnlyList<PlatformDisplayDescription> EnumerateDisplayDevices()
    {
        var          result  = new List<PlatformDisplayDescription>();
        DXGIAdapter? adapter = null;
        DXGIOutput?  output  = null;
        try
        {
            for (var adapterIndex = 0u; adapterIndex < 512 && m_factory.TryEnumAdapters(adapterIndex, ref adapter); ++adapterIndex)
            {
                for (var outputIndex = 0u; outputIndex < 512 && adapter!.TryEnumOutputs(outputIndex, ref output); ++outputIndex)
                {
                    if (output!.TryGetDescription(out var desc) && desc.AttachedToDesktop == Win32Bool.True)
                    {
                        result.Add(new PlatformDisplayDescription
                                   {
                                       Name                = desc.DeviceName,
                                       DesktopPositionLeft = desc.DesktopCoordinates.Left,
                                       DesktopPositionTop  = desc.DesktopCoordinates.Top,
                                       DesktopSizeWidth    = desc.DesktopCoordinates.Right - desc.DesktopCoordinates.Left,
                                       DesktopSizeHeight   = desc.DesktopCoordinates.Bottom - desc.DesktopCoordinates.Top
                                   });
                    }
                }

                output?.Dispose();
                output = null;
            }
        }
        finally
        {
            output?.Dispose();
            adapter?.Dispose();
        }

        return result;
    }
}