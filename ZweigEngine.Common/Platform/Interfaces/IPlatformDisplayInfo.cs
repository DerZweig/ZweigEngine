using ZweigEngine.Common.Platform.Structures;

namespace ZweigEngine.Common.Platform.Interfaces;

public interface IPlatformDisplayInfo
{
    public IReadOnlyList<PlatformDisplayDescription> EnumerateDisplayDevices();
}