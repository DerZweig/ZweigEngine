using System.Runtime.InteropServices;

namespace ZweigEngine.Common.Video.Structures;

[StructLayout(LayoutKind.Sequential)]
public struct VideoColor
{
	public byte Red;
	public byte Green;
	public byte Blue;
	public byte Alpha;
}

[StructLayout(LayoutKind.Sequential)]
public struct VideoBoolean
{
	private const byte TRUE_VALUE  = 0xFF;
	private const byte FALSE_VALUE = 0;

	private byte m_value;

	public VideoBoolean(bool value)
	{
		m_value = value ? TRUE_VALUE : FALSE_VALUE;
	}
	
	public bool Value
	{
		get => m_value != FALSE_VALUE;
		set => m_value = value ? TRUE_VALUE : FALSE_VALUE;
	}
}