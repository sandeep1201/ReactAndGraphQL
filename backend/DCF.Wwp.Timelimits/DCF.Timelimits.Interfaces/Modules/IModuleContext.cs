using System;
using System.Collections.Generic;
using DCF.Core.Plugins;

namespace DCF.Core.Modules
{
    public interface IModuleContext : IDisposable
    {
        IEnumerable<Type> AllAppPluginTypes { get; }
        IEnumerable<Type> AllCorePluginTypes { get; }
        IEnumerable<IApplicationModule> AllAppModules { get; }
        IEnumerable<Type> AllPluginTypes { get; }
        IPlugin AppHost { get; }
        //ILog Logger { get; }
        IPlugin Plugin { get; }

        T GetPluginModule<T>() where T : IAppModuleService;
        IPluginType GetPluginType(Type type);
    }
}