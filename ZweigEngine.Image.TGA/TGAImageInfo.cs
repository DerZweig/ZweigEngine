using ZweigEngine.Common.Assets;
using ZweigEngine.Common.Assets.Constants;
using ZweigEngine.Image.TGA.Constants;

namespace ZweigEngine.Image.TGA;

internal class TGAImageInfo : IImageReader.IImageInfo
{
	public long                    StreamPosition { get; internal set; }
	public ImagePixelFormat        PixelType { get; internal set; }
	public int                     Height         { get; internal set; }
	public int                     Width          { get; internal set; }
	public TGAImageType            FileType       { get; internal set; }
	public TGAImageDescriptorFlags FileDescriptor { get; internal set; }
}