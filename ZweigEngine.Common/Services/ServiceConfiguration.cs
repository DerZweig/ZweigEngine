namespace ZweigEngine.Common.Services;

public class ServiceConfiguration
{
    private readonly List<Step> m_types;

    public ServiceConfiguration()
    {
        m_types = new List<Step>();
    }

    internal IEnumerable<Step> Enumerate()
    {
        return m_types.AsEnumerable();
    }

    public void AddSingleton<TImplementation>()
    {
        m_types.Add(new Step
        {
            type  = typeof(TImplementation),
            alias = null
        });
    }

    public void AddSingleton<TInterface, TImplementation>() where TImplementation : TInterface
    {
        m_types.Add(new Step
        {
            type  = typeof(TImplementation),
            alias = typeof(TInterface)
        });
    }
    
    public struct Step
    {
        public Type  type;
        public Type? alias;
    }
}