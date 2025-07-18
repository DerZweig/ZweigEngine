﻿using ZweigEngine.Platform.Windows.Win32.Structures;

namespace ZweigEngine.Platform.Windows.Win32.Prototypes;

internal delegate int PfnChoosePixelFormatDelegate(IntPtr deviceContext, ref Win32PixelFormatDescriptor pixelFormatDescriptor);

internal delegate bool PfnSetPixelFormatDelegate(IntPtr deviceContext, int pixelFormat, ref Win32PixelFormatDescriptor pixelFormatDescriptor);

internal delegate void PfnSwapBuffersDelegate(IntPtr deviceContext);