using ZweigEngine.Common.Image.Constants;

namespace ZweigEngine.Common.Image.Interfaces;

public interface IImageInfo
{
	ImagePixelFormat PixelType { get; }
	int              Width          { get; }
	int              Height         { get; }
}
