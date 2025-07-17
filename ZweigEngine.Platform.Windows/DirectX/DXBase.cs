using System.Runtime.InteropServices;

namespace ZweigEngine.Platform.Windows.DirectX;

internal abstract class DXBase : IDisposable
{	
    protected delegate IntPtr MethodSelector<TTableType>(in TTableType table) where TTableType : struct;

    private IntPtr  m_pointer;
    private object? m_vtable;

    protected DXBase(IntPtr pointer)
    {
        m_pointer = pointer;
    }

    ~DXBase()
    {
        ReleaseUnmanagedResources();
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }
    
    private void ReleaseUnmanagedResources()
    {
        if (m_pointer != IntPtr.Zero)
        {
            Marshal.Release(m_pointer);
            m_pointer = IntPtr.Zero;
            m_vtable  = null;
        }
    }
    
    public IntPtr Self => m_pointer;

    protected void LoadMethod<TTableType, TDelegate>(MethodSelector<TTableType> selector, out TDelegate method) where TDelegate : Delegate where TTableType : struct
    {
        if (m_pointer == IntPtr.Zero)
        {
            method = null!;
        }
        else
        {
            if (m_vtable == null)
            {
                var tablePointer = Marshal.PtrToStructure<IntPtr>(m_pointer);//*void***
                m_vtable = Marshal.PtrToStructure<TTableType>(tablePointer);//*void**
            }

            var address = selector((TTableType)m_vtable);
            method = Marshal.GetDelegateForFunctionPointer<TDelegate>(address);
        }
    }
}