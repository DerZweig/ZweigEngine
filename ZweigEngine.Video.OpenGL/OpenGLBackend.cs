using ZweigEngine.Common;
using ZweigEngine.Common.Video.Interfaces;
using ZweigEngine.Video.OpenGL.Prototypes;

namespace ZweigEngine.Video.OpenGL;

public sealed class OpenGLBackend : IDisposable, IVideoBackend
{
    // ReSharper disable InconsistentNaming
    // ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
    private readonly PfnEnableDelegate                      glEnable;
    private readonly PfnDisableDelegate                     glDisable;
    private readonly PfnClearColorDelegate                  glClearColor;
    private readonly PfnClearDepthDelegate                  glClearDepth;
    private readonly PfnClearDelegate                       glClear;
    private readonly PfnBlendColorDelegate                  glBlendColor;
    private readonly PfnBlendFunctionDelegate               glBlendFunc;
    private readonly PfnDepthFunctionDelegate               glDepthFunc;
    private readonly PfnCullFaceDelegate                    glCullFace;
    private readonly PfnFrontFaceDelegate                   glFrontFace;
    private readonly PfnGetErrorDelegate                    glGetError;
    private readonly PfnDepthMaskDelegate                   glDepthMask;
    private readonly PfnColorMaskDelegate                   glColorMask;
    private readonly PfnViewportDelegate                    glViewport;
    private readonly PfnScissorDelegate                     glScissor;
    private readonly PfnCreateProgramDelegate               glCreateProgram;
    private readonly PfnDeleteProgramDelegate               glDeleteProgram;
    private readonly PfnUseProgramDelegate                  glUseProgram;
    private readonly PfnLinkProgramDelegate                 glLinkProgram;
    private readonly PfnGetProgramDelegate                  glGetProgramiv;
    private readonly PfnGetProgramInfoLogDelegate           glGetProgramInfoLog;
    private readonly PfnAttachShaderDelegate                glAttachShader;
    private readonly PfnDetachShaderDelegate                glDetachShader;
    private readonly PfnCreateShaderDelegate                glCreateShader;
    private readonly PfnDeleteShaderDelegate                glDeleteShader;
    private readonly PfnShaderSourceDelegate                glShaderSource;
    private readonly PfnCompileShaderDelegate               glCompileShader;
    private readonly PfnGetShaderDelegate                   glGetShaderiv;
    private readonly PfnGetShaderInfoLogDelegate            glGetShaderInfoLog;
    private readonly PfnGetUniformLocationDelegate          glGetUniformLocation;
    private readonly PfnGetAttributeLocationDelegate        glGetAttribLocation;
    private readonly PfnUniform1IDelegate                   glUniform1i;
    private readonly PfnUniform1FDelegate                   glUniform1f;
    private readonly PfnUniform2FDelegate                   glUniform2f;
    private readonly PfnUniform3FDelegate                   glUniform3f;
    private readonly PfnUniform4FDelegate                   glUniform4f;
    private readonly PfnUniformMatrix4FvDelegate            glUniformMatrix4fv;
    private readonly PfnGenVertexArraysDelegate             glGenVertexArrays;
    private readonly PfnDeleteVertexArraysDelegate          glDeleteVertexArrays;
    private readonly PfnBindVertexArrayDelegate             glBindVertexArray;
    private readonly PfnEnableVertexAttributeArrayDelegate  glEnableVertexAttribArray;
    private readonly PfnDisableVertexAttributeArrayDelegate glDisableVertexAttribArray;
    private readonly PfnVertexAttributePointerDelegate      glVertexAttribPointer;
    private readonly PfnVertexAttributeDivisorDelegate      glVertexAttribDivisor;
    private readonly PfnDrawArraysDelegate                  glDrawArrays;
    private readonly PfnDrawArraysInstancedDelegate         glDrawArraysInstanced;
    private readonly PfnGenBuffersDelegate                  glGenBuffers;
    private readonly PfnDeleteBuffersDelegate               glDeleteBuffers;
    private readonly PfnBindBufferDelegate                  glBindBuffer;
    private readonly PfnBufferDataDelegate                  glBufferData;
    private readonly PfnBufferSubDataDelegate               glBufferSubData;
    private readonly PfnActiveTextureDelegate               glActiveTexture;
    private readonly PfnBindTextureDelegate                 glBindTexture;
    private readonly PfnDeleteTexturesDelegate              glDeleteTextures;
    private readonly PfnGenTexturesDelegate                 glGenTextures;
    private readonly PfnTexImage2DDelegate                  glTexImage2D;
    private readonly PfnTexParameteriDelegate               glTexParameteri;
    private readonly PfnTexSubImage2DDelegate               glTexSubImage2D;

    // ReSharper restore PrivateFieldCanBeConvertedToLocalVariable
    // ReSharper restore InconsistentNaming

    public OpenGLBackend(IVarying<IVideoOutput> output)
    {
        var loader = (IOpenGLLoader)output.Current!;
        loader.LoadFunction(nameof(glEnable), out glEnable);
        loader.LoadFunction(nameof(glDisable), out glDisable);
        loader.LoadFunction(nameof(glClearColor), out glClearColor);
        loader.LoadFunction(nameof(glClearDepth), out glClearDepth);
        loader.LoadFunction(nameof(glClear), out glClear);
        loader.LoadFunction(nameof(glBlendColor), out glBlendColor);
        loader.LoadFunction(nameof(glBlendFunc), out glBlendFunc);
        loader.LoadFunction(nameof(glDepthMask), out glDepthMask);
        loader.LoadFunction(nameof(glDepthFunc), out glDepthFunc);
        loader.LoadFunction(nameof(glCullFace), out glCullFace);
        loader.LoadFunction(nameof(glFrontFace), out glFrontFace);
        loader.LoadFunction(nameof(glGetError), out glGetError);
        loader.LoadFunction(nameof(glColorMask), out glColorMask);
        loader.LoadFunction(nameof(glViewport), out glViewport);
        loader.LoadFunction(nameof(glScissor), out glScissor);
        loader.LoadFunction(nameof(glCreateProgram), out glCreateProgram);
        loader.LoadFunction(nameof(glDeleteProgram), out glDeleteProgram);
        loader.LoadFunction(nameof(glUseProgram), out glUseProgram);
        loader.LoadFunction(nameof(glLinkProgram), out glLinkProgram);
        loader.LoadFunction(nameof(glGetProgramiv), out glGetProgramiv);
        loader.LoadFunction(nameof(glGetProgramInfoLog), out glGetProgramInfoLog);
        loader.LoadFunction(nameof(glAttachShader), out glAttachShader);
        loader.LoadFunction(nameof(glDetachShader), out glDetachShader);
        loader.LoadFunction(nameof(glCreateShader), out glCreateShader);
        loader.LoadFunction(nameof(glDeleteShader), out glDeleteShader);
        loader.LoadFunction(nameof(glShaderSource), out glShaderSource);
        loader.LoadFunction(nameof(glCompileShader), out glCompileShader);
        loader.LoadFunction(nameof(glGetShaderiv), out glGetShaderiv);
        loader.LoadFunction(nameof(glGetShaderInfoLog), out glGetShaderInfoLog);
        loader.LoadFunction(nameof(glGetUniformLocation), out glGetUniformLocation);
        loader.LoadFunction(nameof(glGetAttribLocation), out glGetAttribLocation);
        loader.LoadFunction(nameof(glUniform1i), out glUniform1i);
        loader.LoadFunction(nameof(glUniform1f), out glUniform1f);
        loader.LoadFunction(nameof(glUniform2f), out glUniform2f);
        loader.LoadFunction(nameof(glUniform3f), out glUniform3f);
        loader.LoadFunction(nameof(glUniform4f), out glUniform4f);
        loader.LoadFunction(nameof(glUniformMatrix4fv), out glUniformMatrix4fv);
        loader.LoadFunction(nameof(glGenVertexArrays), out glGenVertexArrays);
        loader.LoadFunction(nameof(glDeleteVertexArrays), out glDeleteVertexArrays);
        loader.LoadFunction(nameof(glBindVertexArray), out glBindVertexArray);
        loader.LoadFunction(nameof(glEnableVertexAttribArray), out glEnableVertexAttribArray);
        loader.LoadFunction(nameof(glDisableVertexAttribArray), out glDisableVertexAttribArray);
        loader.LoadFunction(nameof(glVertexAttribPointer), out glVertexAttribPointer);
        loader.LoadFunction(nameof(glVertexAttribDivisor), out glVertexAttribDivisor);
        loader.LoadFunction(nameof(glDrawArrays), out glDrawArrays);
        loader.LoadFunction(nameof(glDrawArraysInstanced), out glDrawArraysInstanced);
        loader.LoadFunction(nameof(glGenBuffers), out glGenBuffers);
        loader.LoadFunction(nameof(glDeleteBuffers), out glDeleteBuffers);
        loader.LoadFunction(nameof(glBindBuffer), out glBindBuffer);
        loader.LoadFunction(nameof(glBufferData), out glBufferData);
        loader.LoadFunction(nameof(glBufferSubData), out glBufferSubData);
        loader.LoadFunction(nameof(glActiveTexture), out glActiveTexture);
        loader.LoadFunction(nameof(glBindTexture), out glBindTexture);
        loader.LoadFunction(nameof(glDeleteTextures), out glDeleteTextures);
        loader.LoadFunction(nameof(glGenTextures), out glGenTextures);
        loader.LoadFunction(nameof(glTexImage2D), out glTexImage2D);
        loader.LoadFunction(nameof(glTexParameteri), out glTexParameteri);
        loader.LoadFunction(nameof(glTexSubImage2D), out glTexSubImage2D);
    }

    private void ReleaseUnmanagedResources()
    {
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~OpenGLBackend()
    {
        ReleaseUnmanagedResources();
    }
}