using System.Runtime.InteropServices;
using ZweigEngine.Common.Platform;
using ZweigEngine.Common.Utility.Interop;
using ZweigEngine.Win32.DirectX.DXGI.Constants;
using ZweigEngine.Win32.DirectX.VTables.DXGI;
using ZweigEngine.Win32.Structures;

namespace ZweigEngine.Win32.DirectX.DXGI;

internal sealed class DXGIFactory : DXObject
{
    private delegate Win32HResult PfnCreateDXGIFactory(IntPtr guid, out IntPtr result);

    private delegate Win32HResult PfnEnumAdaptersDelegate(IntPtr self, uint index, out IntPtr adapter);

    private delegate Win32HResult PfnMakeWindowAssociationDelegate(IntPtr self, IntPtr window, DXGIMakeWindowAssociationFlags flags);

    private readonly PfnEnumAdaptersDelegate          m_enumAdapters;
    private readonly PfnMakeWindowAssociationDelegate m_makeWindowAssociation;

    private DXGIFactory(IntPtr pointer) : base(pointer)
    {
        LoadMethod((in DXGIFactoryMethodTable table) => table.EnumAdapters, out m_enumAdapters);
        LoadMethod((in DXGIFactoryMethodTable table) => table.MakeWindowAssociation, out m_makeWindowAssociation);
    }

    public static bool TryCreate(PlatformLibraryLoader loader, out DXGIFactory factory)
    {
        factory = null!;

        var uuid = typeof(DXGIFactoryMethodTable).GUID;
        using (var pinned = new PinnedObject<Guid>(uuid, GCHandleType.Pinned))
        {
            loader.LoadFunction("dxgi", "CreateDXGIFactory", out PfnCreateDXGIFactory construct);
            if (construct(pinned.GetAddress(), out var pointer).Success)
            {
                factory = new DXGIFactory(pointer);
                return true;
            }
        }

        return false;
    }

    public bool TryEnumAdapters(uint index, ref DXGIAdapter? adapter)
    {
        adapter?.Dispose();
        adapter = null;
        if (m_enumAdapters(Self, index, out var result).Success)
        {
            adapter = new DXGIAdapter(result);
            return true;
        }

        return false;
    }

    public bool TryMakeWindowAssociation(IntPtr window, DXGIMakeWindowAssociationFlags flags)
    {
        return m_makeWindowAssociation(Self, window, flags).Success;
    }
}