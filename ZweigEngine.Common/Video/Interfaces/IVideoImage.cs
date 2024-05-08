using ZweigEngine.Common.Video.Structures;

namespace ZweigEngine.Common.Video.Interfaces;

public interface IVideoImage
{
    int GetWidth();
    int GetHeight();

    void Map(Action<VideoColor[]> mapper);
}