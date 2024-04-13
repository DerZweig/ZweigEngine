﻿namespace ZweigEngine.OpenGL.Constants;

[Flags]
internal enum OpenGLBlendDestinationFactor
{
	Zero             = 0,
	One              = 1,
	SrcColor         = 0x0300,
	OneMinusSrcColor = 0x0301,
	DstColor         = 0x0306,
	OneMinusDstColor = 0x0307,
	SrcAlpha         = 0x0302,
	OneMinusSrcAlpha = 0x0303,
	DstAlpha         = 0x0304,
	OneMinusDstAlpha = 0x0305
}