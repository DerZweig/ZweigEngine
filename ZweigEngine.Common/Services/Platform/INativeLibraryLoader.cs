namespace ZweigEngine.Common.Services.Platform;

public interface INativeLibraryLoader
{
    void LoadFunction<TDelegate>(string libraryPath, string exportName, out TDelegate func) where TDelegate : Delegate;
    bool TryLoadFunction<TDelegate>(string libraryPath, string exportName, out TDelegate func) where TDelegate : Delegate;
}