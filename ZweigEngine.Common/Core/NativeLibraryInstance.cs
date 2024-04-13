﻿using System.Runtime.InteropServices;

namespace ZweigEngine.Common.Core;

internal sealed class NativeLibraryInstance : IDisposable
{
	private readonly IntPtr                      m_nativeHandle;
	private readonly Dictionary<string, object?> m_exported;

	public NativeLibraryInstance(string libraryPath)
	{
		if (NativeLibrary.TryLoad(libraryPath, out var handle))
		{
			m_nativeHandle = handle;
		}

		m_exported = new Dictionary<string, object?>();
	}

	private void ReleaseUnmanagedResources()
	{
		if (IsLoaded)
		{
			NativeLibrary.Free(m_nativeHandle);
		}

		m_exported.Clear();
	}

	public void Dispose()
	{
		ReleaseUnmanagedResources();
		GC.SuppressFinalize(this);
	}

	~NativeLibraryInstance()
	{
		ReleaseUnmanagedResources();
	}

	public bool IsLoaded => m_nativeHandle != IntPtr.Zero;

	public void LoadFunction<TDelegate>(string exportName, out TDelegate func) where TDelegate : Delegate
	{
		if (!TryLoadFunction<TDelegate>(exportName, out var temp))
		{
			throw new Exception($"Couldn't load required function {exportName}.");
		}
		else
		{
			func = temp;
		}
	}

	public bool TryLoadFunction<TDelegate>(string exportName, out TDelegate func) where TDelegate : Delegate
	{
		TDelegate? temp = null;
		if (m_exported.TryGetValue(exportName, out var previous))
		{
			temp = (TDelegate?)previous;
		}
		else
		{
			if (!IsLoaded || !NativeLibrary.TryGetExport(m_nativeHandle, exportName, out var address) || address == IntPtr.Zero)
			{
				temp = null;
			}
			else
			{
				temp = Marshal.GetDelegateForFunctionPointer<TDelegate>(address);
			}

			m_exported[exportName] = temp;
		}

		func = temp!;
		return temp != null;
	}
}