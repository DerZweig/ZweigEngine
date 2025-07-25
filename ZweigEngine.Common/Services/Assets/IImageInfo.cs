using ZweigEngine.Common.Services.Assets.Constants;

namespace ZweigEngine.Common.Services.Assets;

public interface IImageInfo
{
    ImagePixelFormat PixelType { get; }
    int              Width     { get; }
    int              Height    { get; }
}