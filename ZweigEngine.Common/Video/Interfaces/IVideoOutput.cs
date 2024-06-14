namespace ZweigEngine.Common.Video.Interfaces;

public interface IVideoOutput
{
    void Resize(int width, int height);
    void Present();
}