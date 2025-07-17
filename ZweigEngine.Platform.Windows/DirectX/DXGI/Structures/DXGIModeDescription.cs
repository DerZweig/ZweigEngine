﻿using System.Runtime.InteropServices;
using ZweigEngine.Platform.Windows.DirectX.DXGI.Constants;

namespace ZweigEngine.Platform.Windows.DirectX.DXGI.Structures;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct DXGIModeDescription
{
	public uint                  Width;
	public uint                  Height;
	public DXGIRational          RefreshRate;
	public DXGIFormat            Format;
	public DXGIModeScanlineOrder ScanlineOrdering;
	public DXGIModeScaling       Scaling;
}