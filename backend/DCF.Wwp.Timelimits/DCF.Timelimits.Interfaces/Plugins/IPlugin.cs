using System;
using System.Collections.Generic;
using DCF.Core.Container;
using DCF.Core.Modules;

namespace DCF.Core.Plugins
{
    public interface IPlugin
    {
        String AssemblyName { get; }
        Type[] DiscoveredTypes { get; }
        IPluginManifest Manifest { get; }
        IList<IContainerConfig> PluginConfigs { get; }
        IApplicationModule[] AppModules { get; set; }
        PluginTypes PluginType { get; }
        IPluginType[] PluginTypes { get; set; }

        IEnumerable<T> CreatedFrom<T>(IEnumerable<T> instances);
        T GetConfig<T>() where T : IContainerConfig, new();
        T GetRequiredConfig<T>() where T : IContainerConfig, new();
        Boolean IsConfigSet<T>() where T : IContainerConfig;

        IEnumerable<IApplicationModule> IncludedModules();
    }
}