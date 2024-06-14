using ZweigEngine.Common.Video.Constants;
using ZweigEngine.Common.Video.Structures;

namespace ZweigEngine.Common.Video.Interfaces;

public interface IVideoImage
{
    ushort Width  { get; }
    ushort Height { get; }

    void Map(Action<VideoColor[]> mapper);
    void Blit(in VideoRect dstRegion, in VideoRect srcRegion, in VideoColor tintColor, VideoBlitFlags blitFlags);
}