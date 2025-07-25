using System.Runtime.InteropServices;
using ZweigEngine.Common.Services.Platform;
using ZweigEngine.Platform.Windows.Win32.Constants;
using ZweigEngine.Platform.Windows.Win32.Structures;

namespace ZweigEngine.Platform.Windows.Win32.Libraries;

internal class User32
{
    public User32(INativeLibraryLoader loader)
    {
        loader.LoadFunction("user32", "CreateWindowExW", out CreateWindowEx);
        loader.LoadFunction("user32", "DefWindowProcW", out DefaultWindowProc);
        loader.LoadFunction("user32", "DestroyWindow", out DestroyWindow);
        loader.LoadFunction("user32", "DispatchMessageW", out DispatchMessage);
        loader.LoadFunction("user32", "GetWindowRect", out GetWindowRect);
        loader.LoadFunction("user32", "GetClientRect", out GetClientRect);
        loader.LoadFunction("user32", "LoadCursorW", out LoadCursor);
        loader.LoadFunction("user32", "LoadIconW", out LoadIcon);
        loader.LoadFunction("user32", "MsgWaitForMultipleObjects", out MsgWaitForMultipleObjects);
        loader.LoadFunction("user32", "PeekMessageW", out PeekMessage);
        loader.LoadFunction("user32", "RegisterClassExW", out RegisterClassEx);
        loader.LoadFunction("user32", "SetFocus", out SetFocus);
        loader.LoadFunction("user32", "SetForegroundWindow", out SetForegroundWindow);
        loader.LoadFunction("user32", "SetWindowPos", out SetWindowPos);
        loader.LoadFunction("user32", "SetWindowTextW", out SetWindowText);
        loader.LoadFunction("user32", "ShowWindow", out ShowWindow);
        loader.LoadFunction("user32", "TranslateMessage", out TranslateMessage);
        loader.LoadFunction("user32", "UnregisterClassW", out UnregisterClass);
        loader.LoadFunction("user32", "GetWindowLongW", out GetWindowLong);
        loader.LoadFunction("user32", "SetWindowLongW", out SetWindowLong);
        loader.LoadFunction("user32", "BeginPaint", out BeginPaint);
        loader.LoadFunction("user32", "EndPaint", out EndPaint);
        loader.LoadFunction("user32", "InvalidateRect", out InvalidateRect);
        loader.LoadFunction("user32", "GetMessageTime", out GetMessageTime);
        loader.LoadFunction("user32", "GetDC", out GetDeviceContext);
        loader.LoadFunction("user32", "ReleaseDC", out ReleaseDeviceContext);
        loader.LoadFunction("user32", "MapVirtualKeyW", out MapVirtualKey);
        loader.LoadFunction("user32", "GetAsyncKeyState", out GetAsyncKeyState);
        loader.LoadFunction("user32", "GetKeyState", out GetKeyState);
    }

    public readonly PfnBeginPaintDelegate                BeginPaint;
    public readonly PfnCreateWindowExDelegate            CreateWindowEx;
    public readonly PfnDefaultWindowDelegate             DefaultWindowProc;
    public readonly PfnDestroyWindowDelegate             DestroyWindow;
    public readonly PfnDispatchMessageDelegate           DispatchMessage;
    public readonly PfnEndPaintDelegate                  EndPaint;
    public readonly PfnGetClientRectDelegate             GetClientRect;
    public readonly PfnGetMessageTimeDelegate            GetMessageTime;
    public readonly PfnGetWindowLongDelegate             GetWindowLong;
    public readonly PfnGetWindowRectDelegate             GetWindowRect;
    public readonly PfnInvalidateRectDelegate            InvalidateRect;
    public readonly PfnLoadCursorDelegate                LoadCursor;
    public readonly PfnLoadIconDelegate                  LoadIcon;
    public readonly PfnMsgWaitForMultipleObjectsDelegate MsgWaitForMultipleObjects;
    public readonly PfnPeekMessageDelegate               PeekMessage;
    public readonly PfnRegisterClassExDelegate           RegisterClassEx;
    public readonly PfnSetFocusDelegate                  SetFocus;
    public readonly PfnSetForegroundWindowDelegate       SetForegroundWindow;
    public readonly PfnSetWindowLongDelegate             SetWindowLong;
    public readonly PfnSetWindowPosDelegate              SetWindowPos;
    public readonly PfnSetWindowTextDelegate             SetWindowText;
    public readonly PfnShowWindowDelegate                ShowWindow;
    public readonly PfnTranslateMessageDelegate          TranslateMessage;
    public readonly PfnUnregisterClassDelegate           UnregisterClass;
    public readonly PfnGetDeviceContextDelegate          GetDeviceContext;
    public readonly PfnReleaseDeviceContextDelegate      ReleaseDeviceContext;
    public readonly PfnMapVirtualKeyDelegate             MapVirtualKey;
    public readonly PfnGetKeyStateDelegate               GetAsyncKeyState;
    public readonly PfnGetKeyStateDelegate               GetKeyState;

    public delegate IntPtr PfnBeginPaintDelegate(IntPtr windowHandle, ref Win32PaintStruct paintStruct);

    public delegate IntPtr PfnCreateWindowExDelegate(Win32WindowExtendedStyles extendedStyles,
                                                     [MarshalAs(UnmanagedType.LPWStr)] string className,
                                                     [MarshalAs(UnmanagedType.LPWStr)] string windowTitle,
                                                     Win32WindowStyles windowStyles,
                                                     int left,
                                                     int right,
                                                     int width,
                                                     int height,
                                                     IntPtr parentHandle,
                                                     IntPtr menuHandle,
                                                     IntPtr instanceHandle,
                                                     IntPtr parameter);

    public delegate IntPtr PfnDefaultWindowDelegate(IntPtr window, Win32MessageType message, IntPtr wParam, IntPtr lParam);

    public delegate bool PfnDestroyWindowDelegate(IntPtr windowHandle);

    public delegate IntPtr PfnDispatchMessageDelegate(ref Win32Message message);

    public delegate bool PfnEndPaintDelegate(IntPtr windowHandle, ref Win32PaintStruct paintStruct);

    public delegate bool PfnGetWindowRectDelegate(IntPtr windowHandle, ref Win32Rectangle rectangle);

    public delegate bool PfnGetClientRectDelegate(IntPtr windowHandle, ref Win32Rectangle rectangle);

    public delegate IntPtr PfnGetDeviceContextDelegate(IntPtr windowHandle);

    public delegate short PfnGetKeyStateDelegate(Win32VirtualKey virtualKey);

    public delegate long PfnGetMessageTimeDelegate();

    public delegate int PfnGetWindowLongDelegate(IntPtr windowHandle, Win32WindowLongIndex index);

    public delegate bool PfnInvalidateRectDelegate(IntPtr windowHandle, IntPtr rectanglePointer, bool eraseBackground);

    public delegate IntPtr PfnLoadCursorDelegate(IntPtr instanceHandle, IntPtr resource);

    public delegate IntPtr PfnLoadIconDelegate(IntPtr instanceHandle, IntPtr resource);

    public delegate uint PfnMapVirtualKeyDelegate(uint code, Win32MapVirtualKeyType mapType);

    public delegate uint PfnMsgWaitForMultipleObjectsDelegate(uint nCount, IntPtr[] handles, bool waitAll, uint milliseconds, Win32QueueStatusFlags wakeMask);

    public delegate bool PfnPeekMessageDelegate(ref Win32Message message, IntPtr windowHandle, uint messageFilterMin, uint messageFilterMax, Win32PeekMessageFlags remove);

    public delegate ushort PfnRegisterClassExDelegate(ref Win32WindowClassEx windowClass);

    public delegate bool PfnReleaseDeviceContextDelegate(IntPtr windowHandle, IntPtr deviceContext);

    public delegate IntPtr PfnSetFocusDelegate(IntPtr windowHandle);

    public delegate bool PfnSetForegroundWindowDelegate(IntPtr windowHandle);

    public delegate int PfnSetWindowLongDelegate(IntPtr windowHandle, Win32WindowLongIndex index, int value);

    public delegate bool PfnSetWindowPosDelegate(IntPtr windowHandle, IntPtr insertAfter, int x, int y, int width, int height, Win32SetWindowPositionCommands commands);

    public delegate bool PfnSetWindowTextDelegate(IntPtr windowHandle, [MarshalAs(UnmanagedType.LPWStr)] string text);

    public delegate bool PfnShowWindowDelegate(IntPtr windowHandle, Win32ShowWindowCommands commands);

    public delegate IntPtr PfnTranslateMessageDelegate(ref Win32Message message);

    public delegate bool PfnUnregisterClassDelegate([MarshalAs(UnmanagedType.LPWStr)] string className, IntPtr instanceHandle);
}