using ZweigEngine.Common.Assets.Constants;

namespace ZweigEngine.Common.Assets;

public interface IImageReader
{
    public interface IImageInfo
    {
        ImagePixelFormat PixelType { get; }
        int              Width     { get; }
        int              Height    { get; }
    }

    Task<IImageInfo> LoadInfoBlockAsync(Stream stream, CancellationToken cancellationToken);

    Task<IReadOnlyList<byte>> LoadPixelDataAsync(Stream stream, IImageInfo imageInfo, CancellationToken cancellationToken);
}