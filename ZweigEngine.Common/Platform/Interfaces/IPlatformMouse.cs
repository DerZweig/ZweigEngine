using ZweigEngine.Common.Platform.Constants;

namespace ZweigEngine.Common.Platform.Interfaces;

public delegate void PlatformMouseMovedDelegate(IPlatformMouse mouse, int left, int top);

public delegate void PlatformMousePressedDelegate(IPlatformMouse mouse, int left, int top, MouseButton button);

public delegate void PlatformMouseReleasedDelegate(IPlatformMouse mouse, int left, int top, MouseButton button);

public delegate void PlatformMouseScrolledDelegate(IPlatformMouse mouse, int left, int top, int offset);

public interface IPlatformMouse
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