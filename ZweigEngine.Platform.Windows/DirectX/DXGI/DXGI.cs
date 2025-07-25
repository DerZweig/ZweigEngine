using System.Runtime.InteropServices;
using ZweigEngine.Common.Services.Platform;
using ZweigEngine.Common.Utility.Interop;
using ZweigEngine.Platform.Windows.DirectX.DXGI.VTables;
using ZweigEngine.Platform.Windows.Win32.Structures;

namespace ZweigEngine.Platform.Windows.DirectX.DXGI;

internal sealed class DXGI
{
    private delegate Win32HResult PfnCreateDXGIFactory(IntPtr guid, out IntPtr result);

    private readonly PfnCreateDXGIFactory m_createFactory;

    public DXGI(INativeLibraryLoader loader)
    {
        loader.LoadFunction("dxgi", "CreateDXGIFactory", out m_createFactory);
    }

    public bool TryCreateFactory(ref DXGIFactory? factory)
    {
        factory?.Dispose();
        factory = null;
        
        var uuid = typeof(DXGIFactoryMethodTable).GUID;

        using (var pinned = new PinnedObject<Guid>(uuid, GCHandleType.Pinned))
        {
            if (m_createFactory(pinned.GetAddress(), out var pointer).Success)
            {
                factory = new DXGIFactory(pointer);
                return true;
            }
        }

        return false;
    }
}