namespace ZweigEngine.Video.OpenGL;

public interface IOpenGLLoader
{
    void LoadFunction<TDelegate>(string exportName, out TDelegate func) where TDelegate : Delegate;
    bool TryLoadFunction<TDelegate>(string exportName, out TDelegate func) where TDelegate : Delegate;
}