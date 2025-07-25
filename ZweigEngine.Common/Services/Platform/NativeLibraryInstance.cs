using System.Runtime.InteropServices;
using ZweigEngine.Common.Utility;

namespace ZweigEngine.Common.Services.Platform;

internal sealed class NativeLibraryInstance : DisposableObject
{
    private readonly IntPtr                    m_nativeHandle;
    private readonly Dictionary<string, Entry> m_exported;

    public NativeLibraryInstance(string libraryPath)
    {
        if (NativeLibrary.TryLoad(libraryPath, out var handle))
        {
            m_nativeHandle = handle;
        }

        m_exported = new Dictionary<string, Entry>();
    }

    protected override void ReleaseUnmanagedResources()
    {
        if (IsLoaded)
        {
            NativeLibrary.Free(m_nativeHandle);
        }

        m_exported.Clear();
    }

    public bool IsLoaded => m_nativeHandle != IntPtr.Zero;

    public void LoadFunction<TDelegate>(string exportName, out TDelegate func) where TDelegate : Delegate
    {
        if (!TryLoadFunction<TDelegate>(exportName, out var temp))
        {
            throw new Exception($"Couldn't load required function {exportName}.");
        }
        else
        {
            func = temp;
        }
    }

    public bool TryLoadFunction<TDelegate>(string exportName, out TDelegate func) where TDelegate : Delegate
    {
        TDelegate? temp = null;
        if (IsLoaded)
        {
            if (m_exported.TryGetValue(exportName, out var previous) && previous.Type == typeof(TDelegate))
            {
                temp = (TDelegate?)previous.Value;
            }
            else
            {
                if (!NativeLibrary.TryGetExport(m_nativeHandle, exportName, out var address) || address == IntPtr.Zero)
                {
                    temp = null;
                }
                else
                {
                    temp = Marshal.GetDelegateForFunctionPointer<TDelegate>(address);
                }

                m_exported[exportName] = new Entry { Value = temp, Type = typeof(TDelegate) };
            }
        }

        func = temp!;
        return temp != null;
    }

    private class Entry
    {
        public object? Value { get; set; }
        public Type    Type  { get; set; }
    }
}