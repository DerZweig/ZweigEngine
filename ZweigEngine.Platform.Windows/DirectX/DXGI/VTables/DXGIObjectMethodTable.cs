﻿using System.Runtime.InteropServices;

namespace ZweigEngine.Platform.Windows.DirectX.DXGI.VTables;

[Guid("aec22fb8-76f3-4639-9be0-28eb43a67a2e")]
[StructLayout(LayoutKind.Sequential)]
internal struct DXGIObjectMethodTable
{
	public UnknownMethodTable Super;
	public IntPtr             SetPrivateData;
	public IntPtr             SetPrivateDateInterface;
	public IntPtr             GetPrivateData;
	public IntPtr             GetParent;
}