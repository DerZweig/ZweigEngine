using System.Runtime.InteropServices;

namespace ZweigEngine.Platform.Windows.Win32.Structures;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct Win32DeviceMode
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string DeviceName;

    public ushort SpecVersion;
    public ushort DriverVersion;
    public ushort Size;
    public ushort DriverExtra;
    public int    Fields;

    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I2, SizeConst = 8)]
    private ushort[] union1;

    public short Color;
    public short Duplex;
    public short ResolutionVertical;
    public short TOption;
    public short Collate;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string FormName;

    public  short LogicalPixels;
    public  int   BitsPerPel;
    public  int   PelsWidth;
    public  int   PelsHeight;
    private int   union2;
    public  int   DisplayFrequency;
    public  int   ICMMethod;
    public  int   ICMIntent;
    public  int   MediaType;
    public  int   Reserved1;
    public  int   Reserved2;
    public  int   PanningWidth;
    public  int   PanningHeight;
}