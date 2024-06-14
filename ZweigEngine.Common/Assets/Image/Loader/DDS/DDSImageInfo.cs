using ZweigEngine.Common.Assets.Image.Constants;
using ZweigEngine.Common.Assets.Image.Interfaces;
using ZweigEngine.Common.Assets.Image.Loader.DDS.Constants;
using ZweigEngine.Common.Assets.Image.Loader.DDS.Decoder;

namespace ZweigEngine.Common.Assets.Image.Loader.DDS;

internal sealed class DDSImageInfo : IImageInfo
{
	public long             StreamPosition { get; internal set; }
	public ImagePixelFormat PixelType      { get; internal set; }
	public int              Height         { get; internal set; }
	public int              Width          { get; internal set; }
	public DDSImageFormat   FileFormat     { get; internal set; }
	public DDSDecoder       FileDecoder    { get; internal set; } = null!;
}