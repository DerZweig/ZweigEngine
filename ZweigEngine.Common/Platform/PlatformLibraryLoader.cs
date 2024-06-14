namespace ZweigEngine.Common.Platform;

public sealed class PlatformLibraryLoader : IDisposable
{
	private readonly Dictionary<string, PlatformLibraryInstance> m_libraries;

	public PlatformLibraryLoader()
	{
		m_libraries = new Dictionary<string, PlatformLibraryInstance>();
	}

	private void ReleaseUnmanagedResources()
	{
		var instances = m_libraries.Values;
		m_libraries.Clear();

		foreach (var instance in instances)
		{
			instance.Dispose();
		}
	}

	public void Dispose()
	{
		ReleaseUnmanagedResources();
		GC.SuppressFinalize(this);
	}

	~PlatformLibraryLoader()
	{
		ReleaseUnmanagedResources();
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

	private PlatformLibraryInstance GetOrAddLibraryInstance(string libraryPath)
	{
		if (!m_libraries.TryGetValue(libraryPath, out var instance))
		{
			instance                 = new PlatformLibraryInstance(libraryPath);
			m_libraries[libraryPath] = instance;
		}

		return instance;
	}
}