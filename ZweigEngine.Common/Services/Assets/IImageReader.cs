namespace ZweigEngine.Common.Services.Assets;

public interface IImageReader
{
    bool ShouldHandleFilePath(string path);
    
    Task<IImageInfo> LoadInfoBlock(Stream stream, CancellationToken cancellationToken);

    Task<IReadOnlyList<byte>> LoadPixelData(Stream stream, IImageInfo imageInfo, CancellationToken cancellationToken);
}