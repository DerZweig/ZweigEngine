using ZweigEngine.Common.Platform.Constants;

namespace ZweigEngine.Common.Platform;

public delegate void PlatformWindowDelegate(IWindow window);

public interface IWindow
{
    event PlatformWindowDelegate OnCreated;
    event PlatformWindowDelegate OnClosing;
    event PlatformWindowDelegate OnUpdate;

    IntPtr GetNativePointer();
    bool IsAvailable();
    bool IsFocused();
    int GetPositionLeft();
    int GetPositionTop();
    int GetSizeWidth();
    int GetSizeHeight();
    int GetViewportWidth();
    int GetViewportHeight();
    void Close();
    void Create();
    void Update();
    void SetTitle(string text);
    void SetStyle(WindowStyle style);
    void SetPosition(int left, int top);
    void SetSize(int width, int height);
    void SetMinimumSize(int width, int height);
    void SetMaximumSize(int width, int height);
}