﻿using ZweigEngine.Common.Utility;

namespace ZweigEngine.Common.Services.Platform;

internal sealed class NativeLibraryLoader : DisposableObject, INativeLibraryLoader
{
    private readonly Dictionary<string, NativeLibraryInstance> m_libraries;

    public NativeLibraryLoader()
    {
        m_libraries = new Dictionary<string, NativeLibraryInstance>();
    }

    protected override void ReleaseUnmanagedResources()
    {
        var instances = m_libraries.Values;
        m_libraries.Clear();

        foreach (var instance in instances)
        {
            instance.Dispose();
        }
    }

    public void LoadFunction<TDelegate>(string libraryPath, string exportName, out TDelegate func) where TDelegate : Delegate
    {
        var instance = GetOrAddLibraryInstance(libraryPath);
        instance.LoadFunction(exportName, out func);
    }

    public bool TryLoadFunction<TDelegate>(string libraryPath, string exportName, out TDelegate func) where TDelegate : Delegate
    {
        var instance = GetOrAddLibraryInstance(libraryPath);
        if (instance.TryLoadFunction(exportName, out func))
        {
            return true;
        }

        func = null!;
        return false;
    }

    private NativeLibraryInstance GetOrAddLibraryInstance(string libraryPath)
    {
        if (!m_libraries.TryGetValue(libraryPath, out var instance))
        {
            instance                 = new NativeLibraryInstance(libraryPath);
            m_libraries[libraryPath] = instance;
        }

        return instance;
    }
}