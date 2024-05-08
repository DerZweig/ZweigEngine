using ZweigEngine.Common.Platform.Structures;

namespace ZweigEngine.Common.Platform.Interfaces;

public interface IPlatformInfo
{
    public IReadOnlyList<PlatformDisplayDescription> EnumerateDisplayDevices();
}