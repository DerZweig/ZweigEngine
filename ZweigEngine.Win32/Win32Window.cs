﻿using System.Runtime.InteropServices;
using ZweigEngine.Common.Platform;
using ZweigEngine.Common.Platform.Constants;
using ZweigEngine.Common.Platform.Interfaces;
using ZweigEngine.Common.Utility.Interop;
using ZweigEngine.Win32.Constants;
using ZweigEngine.Win32.Prototypes;
using ZweigEngine.Win32.Structures;

namespace ZweigEngine.Win32;

public class Win32Window : IDisposable, IPlatformWindow
{
    private const ushort                    INVALID_ATOM               = 0;
    private const Win32WindowStyles         WINDOW_BASE_STYLE          = Win32WindowStyles.ClipChildren | Win32WindowStyles.ClipSiblings;
    private const Win32WindowStyles         WINDOW_BORDER_STYLE        = WINDOW_BASE_STYLE | Win32WindowStyles.Overlapped | Win32WindowStyles.Caption | Win32WindowStyles.SystemMenu | Win32WindowStyles.MinimizeBox | Win32WindowStyles.ThickFrame | Win32WindowStyles.MaximizeBox;
    private const Win32WindowStyles         WINDOW_BORDERLESS_STYLE    = WINDOW_BASE_STYLE | Win32WindowStyles.Popup;
    private const Win32WindowExtendedStyles WINDOW_BORDER_STYLE_EX     = Win32WindowExtendedStyles.ClientEdge | Win32WindowExtendedStyles.AppWindow;
    private const Win32WindowExtendedStyles WINDOW_BORDERLESS_STYLE_EX = Win32WindowExtendedStyles.AppWindow;
    private const Win32WindowStyles         WINDOW_STYLE_MASK          = WINDOW_BORDER_STYLE | WINDOW_BORDERLESS_STYLE;
    private const Win32WindowExtendedStyles WINDOW_STYLE_MASK_EX       = WINDOW_BORDER_STYLE_EX | WINDOW_BORDERLESS_STYLE_EX;
    private const string                    WINDOW_DEFAULT_TITLE       = "Untitled";
    private const string                    WINDOW_CLASS_NAME          = "ZweigEngine::WindowClass";
    private const Win32ClassStyles          WINDOW_CLASS_STYLE         = Win32ClassStyles.HorizontalRedraw | Win32ClassStyles.VerticalRedraw | Win32ClassStyles.OwnDeviceContext;

    // ReSharper disable InconsistentNaming
    private readonly PfnBeginPaintDelegate                BeginPaint;
    private readonly PfnCreateWindowExDelegate            CreateWindowEx;
    private readonly PfnDefaultWindowDelegate             DefaultWindowProc;
    private readonly PfnDestroyWindowDelegate             DestroyWindow;
    private readonly PfnDispatchMessageDelegate           DispatchMessage;
    private readonly PfnEndPaintDelegate                  EndPaint;
    private readonly PfnGetClientRectDelegate             GetClientRect;
    private readonly PfnGetMessageTimeDelegate            GetMessageTime;
    private readonly PfnGetModuleHandleDelegate           GetModuleHandle;
    private readonly PfnGetWindowLongDelegate             GetWindowLong;
    private readonly PfnGetWindowRectDelegate             GetWindowRect;
    private readonly PfnInvalidateRectDelegate            InvalidateRect;
    private readonly PfnLoadCursorDelegate                LoadCursor;
    private readonly PfnLoadIconDelegate                  LoadIcon;
    private readonly PfnMsgWaitForMultipleObjectsDelegate MsgWaitForMultipleObjects;
    private readonly PfnPeekMessageDelegate               PeekMessage;
    private readonly PfnRegisterClassExDelegate           RegisterClassEx;
    private readonly PfnSetFocusDelegate                  SetFocus;
    private readonly PfnSetForegroundWindowDelegate       SetForegroundWindow;
    private readonly PfnSetWindowLongDelegate             SetWindowLong;
    private readonly PfnSetWindowPosDelegate              SetWindowPos;
    private readonly PfnSetWindowTextDelegate             SetWindowText;
    private readonly PfnShowWindowDelegate                ShowWindow;
    private readonly PfnTranslateMessageDelegate          TranslateMessage;
    private readonly PfnUnregisterClassDelegate           UnregisterClass;
    // ReSharper restore InconsistentNaming

    private readonly Win32Synchronization          m_synchronization;
    private readonly PinnedDelegate<PfnWindowProc> m_proc;
    private readonly Win32Keyboard                 m_keyboard;
    private readonly Win32Mouse                    m_mouse;
    private          IntPtr                        m_owner;
    private          IntPtr                        m_icon;
    private          IntPtr                        m_cursor;
    private          ushort                        m_class;
    private          IntPtr                        m_handle;
    private          bool                          m_sizing;
    private          bool                          m_closing;
    private          bool                          m_focused;
    private          int                           m_position_left;
    private          int                           m_position_top;
    private          int                           m_size_width;
    private          int                           m_size_height;
    private          int                           m_viewport_width;
    private          int                           m_viewport_height;
    private          int                           m_minimum_width;
    private          int                           m_minimum_height;
    private          int                           m_maximum_width;
    private          int                           m_maximum_height;
    private          Win32Message                  m_message;
    private          Exception?                    m_error;

    public Win32Window(PlatformLibraryLoader libraryLoader, Win32Keyboard keyboard, Win32Mouse mouse, IWin32DPIScalingHandler _)
    {
        m_synchronization = new Win32Synchronization();
        m_proc            = new PinnedDelegate<PfnWindowProc>(Process, GCHandleType.Normal);
        m_keyboard        = keyboard;
        m_mouse           = mouse;
        m_maximum_width   = int.MaxValue;
        m_maximum_height  = int.MaxValue;

        libraryLoader.LoadFunction("kernel32", "GetModuleHandleW", out GetModuleHandle);
        libraryLoader.LoadFunction("user32", "CreateWindowExW", out CreateWindowEx);
        libraryLoader.LoadFunction("user32", "DefWindowProcW", out DefaultWindowProc);
        libraryLoader.LoadFunction("user32", "DestroyWindow", out DestroyWindow);
        libraryLoader.LoadFunction("user32", "DispatchMessageW", out DispatchMessage);
        libraryLoader.LoadFunction("user32", "GetWindowRect", out GetWindowRect);
        libraryLoader.LoadFunction("user32", "GetClientRect", out GetClientRect);
        libraryLoader.LoadFunction("user32", "LoadCursorW", out LoadCursor);
        libraryLoader.LoadFunction("user32", "LoadIconW", out LoadIcon);
        libraryLoader.LoadFunction("user32", "MsgWaitForMultipleObjects", out MsgWaitForMultipleObjects);
        libraryLoader.LoadFunction("user32", "PeekMessageW", out PeekMessage);
        libraryLoader.LoadFunction("user32", "RegisterClassExW", out RegisterClassEx);
        libraryLoader.LoadFunction("user32", "SetFocus", out SetFocus);
        libraryLoader.LoadFunction("user32", "SetForegroundWindow", out SetForegroundWindow);
        libraryLoader.LoadFunction("user32", "SetWindowPos", out SetWindowPos);
        libraryLoader.LoadFunction("user32", "SetWindowTextW", out SetWindowText);
        libraryLoader.LoadFunction("user32", "ShowWindow", out ShowWindow);
        libraryLoader.LoadFunction("user32", "TranslateMessage", out TranslateMessage);
        libraryLoader.LoadFunction("user32", "UnregisterClassW", out UnregisterClass);
        libraryLoader.LoadFunction("user32", "GetWindowLongW", out GetWindowLong);
        libraryLoader.LoadFunction("user32", "SetWindowLongW", out SetWindowLong);
        libraryLoader.LoadFunction("user32", "BeginPaint", out BeginPaint);
        libraryLoader.LoadFunction("user32", "EndPaint", out EndPaint);
        libraryLoader.LoadFunction("user32", "InvalidateRect", out InvalidateRect);
        libraryLoader.LoadFunction("user32", "GetMessageTime", out GetMessageTime);
    }

    private void ReleaseUnmanagedResources()
    {
        CloseInternal();
        if (m_class != INVALID_ATOM)
        {
            UnregisterClass(WINDOW_CLASS_NAME, m_owner);
            m_class = INVALID_ATOM;
        }

        m_proc.Dispose();
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~Win32Window()
    {
        ReleaseUnmanagedResources();
    }

    public event PlatformWindowDelegate? OnCreated;
    public event PlatformWindowDelegate? OnClosing;
    public event PlatformWindowDelegate? OnUpdate;

    public IntPtr GetNativePointer() => m_handle;
    public bool IsAvailable() => m_handle != IntPtr.Zero && !m_closing;
    public bool IsFocused() => m_handle != IntPtr.Zero && m_focused;
    public int GetPositionLeft() => m_position_left;
    public int GetPositionTop() => m_position_top;
    public int GetSizeWidth() => m_size_width;
    public int GetSizeHeight() => m_size_height;
    public int GetViewportWidth() => m_viewport_width;
    public int GetViewportHeight() => m_viewport_height;

    public void Close()
    {
        CloseInternal();
        RethrowError();
    }

    public void Create()
    {
        if (m_owner == IntPtr.Zero)
        {
            m_owner  = GetModuleHandle(IntPtr.Zero);
            m_icon   = LoadIcon(IntPtr.Zero, new IntPtr((int)Win32SystemIcon.Application));
            m_cursor = LoadCursor(IntPtr.Zero, new IntPtr((int)Win32SystemCursor.Arrow));
        }

        if (m_class == INVALID_ATOM)
        {
            var classDesc = new Win32WindowClassEx
                            {
                                Size            = Marshal.SizeOf<Win32WindowClassEx>(),
                                Styles          = WINDOW_CLASS_STYLE,
                                ClassName       = WINDOW_CLASS_NAME,
                                WindowProc      = m_proc.GetAddress(),
                                InstanceHandle  = m_owner,
                                IconHandle      = m_icon,
                                SmallIconHandle = m_icon,
                                CursorHandle    = m_cursor
                            };

            m_class = RegisterClassEx(ref classDesc);
        }

        if (m_handle == IntPtr.Zero && m_class != INVALID_ATOM)
        {
            m_handle = CreateWindowEx(WINDOW_BORDER_STYLE_EX,
                                      WINDOW_CLASS_NAME,
                                      WINDOW_DEFAULT_TITLE,
                                      WINDOW_BORDER_STYLE,
                                      (int)Win32CreateWindowFlags.Usedefault,
                                      (int)Win32CreateWindowFlags.Usedefault,
                                      (int)Win32CreateWindowFlags.Usedefault,
                                      (int)Win32CreateWindowFlags.Usedefault,
                                      IntPtr.Zero,
                                      IntPtr.Zero,
                                      m_owner,
                                      IntPtr.Zero);

            if (IsAvailable())
            {
                HandleResizeMessage();
            }

            if (IsAvailable())
            {
                NotifyWindowCreated();
            }

            if (IsAvailable())
            {
                ShowWindow(m_handle, Win32ShowWindowCommands.ShowDefault);
                SetForegroundWindow(m_handle);
                SetFocus(m_handle);
            }

            RethrowError();
        }
    }

    public void Update()
    {
        while (IsAvailable() && PeekMessage(ref m_message, m_handle, 0, 0, Win32PeekMessageFlags.Remove))
        {
            TranslateMessage(ref m_message);
            DispatchMessage(ref m_message);
            RethrowError();
        }

        if (!IsAvailable())
        {
            return;
        }
        
        NotifyWindowUpdate();
        RethrowError();
        if (!IsFocused())
        {
            MsgWaitForMultipleObjects(0u, [], false, 100, Win32QueueStatusFlags.Allinput);
        }
    }

    public void SetTitle(string text)
    {
        if (!IsAvailable())
        {
            return;
        }

        SetWindowText(m_handle, !string.IsNullOrWhiteSpace(text) ? text : WINDOW_DEFAULT_TITLE);

        RethrowError();
    }

    public void SetStyle(WindowStyle style)
    {
        if (IsAvailable())
        {
            var windowStyle   = GetWindowLong(m_handle, Win32WindowLongIndex.Style);
            var windowStyleEx = GetWindowLong(m_handle, Win32WindowLongIndex.ExtendedStyle);

            windowStyle   &= ~(int)WINDOW_STYLE_MASK;
            windowStyleEx &= ~(int)WINDOW_STYLE_MASK_EX;

            switch (style)
            {
                case WindowStyle.Borderless:
                    windowStyle   |= (int)WINDOW_BORDERLESS_STYLE;
                    windowStyleEx |= (int)WINDOW_BORDERLESS_STYLE_EX;
                    break;
                case WindowStyle.Windowed:
                    windowStyle   |= (int)WINDOW_BORDER_STYLE;
                    windowStyleEx |= (int)WINDOW_BORDER_STYLE_EX;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(style), style, null);
            }

            SetWindowLong(m_handle, Win32WindowLongIndex.Style, windowStyle);
            SetWindowLong(m_handle, Win32WindowLongIndex.ExtendedStyle, windowStyleEx);
            SetWindowPos(m_handle, IntPtr.Zero, 0, 0, 0, 0,
                         Win32SetWindowPositionCommands.FrameChanged |
                         Win32SetWindowPositionCommands.NoCopyBits |
                         Win32SetWindowPositionCommands.NoZOrder |
                         Win32SetWindowPositionCommands.NoSize |
                         Win32SetWindowPositionCommands.NoMove |
                         Win32SetWindowPositionCommands.NoActivate);
            RethrowError();
        }
    }

    public void SetPosition(int left, int top)
    {
        if (IsAvailable())
        {
            if (left != m_position_left || top != m_position_top)
            {
                SetWindowPos(m_handle, IntPtr.Zero, left, top, 0, 0,
                             Win32SetWindowPositionCommands.NoSize |
                             Win32SetWindowPositionCommands.NoZOrder |
                             Win32SetWindowPositionCommands.NoActivate);
            }

            RethrowError();
        }
    }

    public void SetSize(int width, int height)
    {
        if (IsAvailable())
        {
            var cw = Math.Clamp(width, m_minimum_width, m_maximum_width);
            var ch = Math.Clamp(height, m_minimum_height, m_maximum_height);

            if (cw != m_size_width || ch != m_size_height)
            {
                SetWindowPos(m_handle, IntPtr.Zero, 0, 0, cw, ch,
                             Win32SetWindowPositionCommands.NoMove |
                             Win32SetWindowPositionCommands.NoZOrder |
                             Win32SetWindowPositionCommands.NoActivate);
            }

            RethrowError();
        }
    }

    public void SetMinimumSize(int width, int height)
    {
        if (IsAvailable())
        {
            m_minimum_width  = width;
            m_minimum_height = height;
            SetSize(m_size_width, m_size_height);
        }
    }

    public void SetMaximumSize(int width, int height)
    {
        if (IsAvailable())
        {
            m_maximum_width  = width;
            m_maximum_height = height;
            SetSize(m_size_width, m_size_height);
        }
    }
    
    private void NotifyWindowUpdate()
    {
        WrapError(() => m_synchronization.Execute(() =>
        {
            m_keyboard.BeginFrame();
            m_mouse.BeginFrame();
            OnUpdate?.Invoke(this);
            m_synchronization.Execute();
        }));
    }

    private void NotifyWindowCreated()
    {
        m_synchronization.ExecuteWithoutPending(() =>
        {
            m_keyboard.ResetState();
            m_mouse.ResetState();
            WrapError(() => OnCreated?.Invoke(this));
        });
    }

    private void NotifyWindowClosing()
    {
        m_synchronization.ExecuteWithoutPending(() =>
        {
            WrapError(() => OnClosing?.Invoke(this));
            m_keyboard.ResetState();
            m_mouse.ResetState();
        });
    }


    private void NotifyWindowMessage(long lTime, IntPtr hWindow, Win32MessageType uMessage, IntPtr wParam, IntPtr lParam)
    {
        WrapError(() => m_synchronization.ExecuteWithoutPending(() =>
        {
            m_keyboard.Process(lTime, hWindow, uMessage, wParam, lParam);
            m_mouse.Process(lTime, hWindow, uMessage, wParam, lParam);
        }));
    }

    private IntPtr Process(IntPtr hWindow, Win32MessageType uMessage, IntPtr wParam, IntPtr lParam)
    {
        NotifyWindowMessage(GetMessageTime(), hWindow, uMessage, wParam, lParam);
        switch (uMessage)
        {
            case Win32MessageType.KeyDown:
            case Win32MessageType.SysKeyDown:
            case Win32MessageType.KeyUp:
            case Win32MessageType.SysKeyUp:
            case Win32MessageType.MouseMove:
            case Win32MessageType.LeftButtonDown:
            case Win32MessageType.LeftButtonUp:
            case Win32MessageType.MiddleButtonDown:
            case Win32MessageType.MiddleButtonUp:
            case Win32MessageType.RightButtonDown:
            case Win32MessageType.RightButtonUp:
            case Win32MessageType.ExtraButtonDown:
            case Win32MessageType.ExtraButtonUp:
            case Win32MessageType.Char:
            case Win32MessageType.SysChar:
                return IntPtr.Zero;
            case Win32MessageType.KillFocus:
                m_focused = false;
                break;
            case Win32MessageType.SetFocus:
                m_focused = true;
                break;
            case Win32MessageType.ShowWindow:
            case Win32MessageType.Size:
            case Win32MessageType.Move:
                HandleResizeMessage();
                break;
            case Win32MessageType.EnterSizeMove:
                m_sizing = true;
                InvalidateRect(m_handle, IntPtr.Zero, false);
                break;
            case Win32MessageType.ExitSizeMove:
                m_sizing = false;
                HandleResizeMessage();
                break;
            case Win32MessageType.GetMinMaxInfo:
                HandleBoundsMessage(lParam);
                break;
            case Win32MessageType.Paint:
                HandlePaintMessage();
                return IntPtr.Zero;
            case Win32MessageType.Close:
                CloseInternal();
                return IntPtr.Zero;
        }

        return DefaultWindowProc(hWindow, uMessage, wParam, lParam);
    }

    private void CloseInternal()
    {
        if (IsAvailable())
        {
            m_closing = true;
            NotifyWindowClosing();
            DestroyWindow(m_handle);
            m_handle          = IntPtr.Zero;
            m_sizing          = false;
            m_closing         = false;
            m_position_left   = 0;
            m_position_top    = 0;
            m_size_width      = 0;
            m_size_width      = 0;
            m_viewport_width  = 0;
            m_viewport_height = 0;
            m_minimum_width   = 0;
            m_minimum_height  = 0;
            m_maximum_width   = int.MaxValue;
            m_maximum_height  = int.MaxValue;
        }
    }

    private void HandleResizeMessage()
    {
        var rect = new Win32Rectangle();
        if (GetWindowRect(m_handle, ref rect))
        {
            m_position_left = rect.Left;
            m_position_top  = rect.Top;
            m_size_width    = rect.Right - rect.Left;
            m_size_height   = rect.Bottom - rect.Top;
        }

        if (GetClientRect(m_handle, ref rect))
        {
            m_viewport_width  = rect.Right - rect.Left;
            m_viewport_height = rect.Bottom - rect.Top;
        }
    }

    private void HandlePaintMessage()
    {
        var ps = new Win32PaintStruct();
        if (BeginPaint(m_handle, ref ps) != IntPtr.Zero)
        {
            EndPaint(m_handle, ref ps);
        }

        if (m_sizing)
        {
            HandleResizeMessage();
            NotifyWindowUpdate();
            if (m_handle != IntPtr.Zero)
            {
                InvalidateRect(m_handle, IntPtr.Zero, false);
            }
        }
    }

    private void HandleBoundsMessage(IntPtr lParam)
    {
        var minMaxInfo = Marshal.PtrToStructure<Win32MinMaxInfo>(lParam);
        minMaxInfo.MinTrackSize.X = m_minimum_width;
        minMaxInfo.MinTrackSize.Y = m_minimum_height;
        minMaxInfo.MaxTrackSize.X = m_maximum_width;
        minMaxInfo.MaxTrackSize.Y = m_maximum_height;
        Marshal.StructureToPtr(minMaxInfo, lParam, false);
    }

    private void RethrowError()
    {
        if (m_error != null)
        {
            var temp = m_error;
            m_error = null;
            throw temp;
        }
    }

    private void WrapError(Action work)
    {
        try
        {
            work();
        }
        catch (Exception ex)
        {
            m_error = ex;
            CloseInternal();
        }
    }
}