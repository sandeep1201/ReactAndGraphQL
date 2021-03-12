using Castle.Windsor;

namespace DCF.Core.Plugins
{
    public interface ITypeRegistration
    {
        IWindsorContainer PluginTypes { get; }
    }
}