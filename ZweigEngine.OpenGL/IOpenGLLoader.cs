namespace ZweigEngine.OpenGL;

public interface IOpenGLLoader
{
    int GetVersionMajor();
    int GetVersionMinor();
    
    void LoadFunction<TDelegate>(string exportName, out TDelegate func) where TDelegate : Delegate;
    bool TryLoadFunction<TDelegate>(string exportName, out TDelegate func) where TDelegate : Delegate;
}