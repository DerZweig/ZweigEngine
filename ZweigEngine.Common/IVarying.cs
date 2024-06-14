namespace ZweigEngine.Common;

public interface IVarying<out TInterface> where TInterface : class
{
    public TInterface? Current { get; }
    public IEnumerable<string> EnumerateOptions();
    public void Activate(string optionName);
    public void Deactivate();
}