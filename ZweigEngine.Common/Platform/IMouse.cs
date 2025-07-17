using ZweigEngine.Common.Platform.Constants;

namespace ZweigEngine.Common.Platform;

public delegate void PlatformMouseMovedDelegate(IMouse mouse, int left, int top);

public delegate void PlatformMousePressedDelegate(IMouse mouse, int left, int top, MouseButton button);

public delegate void PlatformMouseReleasedDelegate(IMouse mouse, int left, int top, MouseButton button);

public delegate void PlatformMouseScrolledDelegate(IMouse mouse, int left, int top, int offset);

public interface IMouse
{
    event PlatformMouseMovedDelegate    OnMouseMoved;
    event PlatformMousePressedDelegate  OnMousePressed;
    event PlatformMouseReleasedDelegate OnMouseReleased;
    event PlatformMouseScrolledDelegate OnMouseScrolledHorizontal;
    event PlatformMouseScrolledDelegate OnMouseScrolledVertical;

    int  GetPositionLeft();
    int  GetPositionTop();
    bool IsButtonPressed(MouseButton button);
    bool IsButtonReleased(MouseButton button);
}