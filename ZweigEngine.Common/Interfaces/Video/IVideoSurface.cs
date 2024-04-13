namespace ZweigEngine.Common.Interfaces.Video;

public interface IVideoSurface
{
    int GetWidth();
    int GetHeight();
    void Resize(int width, int height);
    void Present();
}