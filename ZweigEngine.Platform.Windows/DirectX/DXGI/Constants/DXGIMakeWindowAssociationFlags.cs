namespace ZweigEngine.Platform.Windows.DirectX.DXGI.Constants;

[Flags]
internal enum DXGIMakeWindowAssociationFlags
{
	NoWindowChanges = 1 << 0,
	NoAltEnter      = 1 << 1,
	NoPrintScreen   = 1 << 2
}