namespace ZweigEngine.Common.Video.Interfaces;

public interface IVideoOutput
{
    int GetWidth();
    int GetHeight();
    void Resize(int width, int height);
    void Present();
}