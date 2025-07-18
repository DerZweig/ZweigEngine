﻿using ZweigEngine.Platform.Windows.DirectX.DXGI.Structures;
using ZweigEngine.Platform.Windows.DirectX.DXGI.VTables;
using ZweigEngine.Platform.Windows.Win32.Structures;

namespace ZweigEngine.Platform.Windows.DirectX.DXGI;

internal sealed class DXGIAdapter : DXBase
{
    private delegate Win32HResult PfnGetDescDelegate(IntPtr self, ref DXGIAdapterDescription desc);
    private delegate Win32HResult PfnEnumOutputsDelegate(IntPtr self, uint index, out IntPtr output);

    private readonly PfnEnumOutputsDelegate m_enumOutputs;
    private readonly PfnGetDescDelegate     m_getDesc;

    internal DXGIAdapter(IntPtr pointer) : base(pointer)
    {
        LoadMethod((in DXGIAdapterMethodTable table) => table.EnumOutputs, out m_enumOutputs);
        LoadMethod((in DXGIAdapterMethodTable table) => table.GetDesc, out m_getDesc);
    }

    public bool TryEnumOutputs(uint index, ref DXGIOutput? output)
    {
        output?.Dispose();
        output = null;
		
        if (m_enumOutputs(Self, index, out var result).Success)
        {
            output = new DXGIOutput(result);
            return true;
        }

        return false;
    }

    public bool TryGetDescription(out DXGIAdapterDescription desc)
    {
        desc = default;
        return m_getDesc(Self, ref desc).Success;
    }
}