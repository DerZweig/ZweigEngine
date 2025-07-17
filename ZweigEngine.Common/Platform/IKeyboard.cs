using ZweigEngine.Common.Platform.Constants;

namespace ZweigEngine.Common.Platform;

public delegate void PlatformKeyPressedDelegate(IKeyboard keyboard, KeyboardKey key);

public delegate void PlatformKeyReleasedDelegate(IKeyboard keyboard, KeyboardKey key);

public delegate void PlatformKeyTypedDelegate(IKeyboard keyboard, char character);

public interface IKeyboard
{
    event PlatformKeyPressedDelegate  OnKeyPressed;
    event PlatformKeyReleasedDelegate OnKeyReleased;
    event PlatformKeyTypedDelegate    OnKeyTyped;

    bool IsKeyPressed(KeyboardKey key);
    bool IsKeyReleased(KeyboardKey key);
}