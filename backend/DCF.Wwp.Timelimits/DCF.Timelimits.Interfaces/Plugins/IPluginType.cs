using System;
using System.Collections.Generic;

namespace DCF.Core.Plugins
{
    public interface IPluginType
    {
        String AssemblyName { get; }
        IEnumerable<IPlugin> DiscoveredByPlugins { get; set; }
        Boolean IsKnownType { get; }
        IPlugin Plugin { get; }
        Type Type { get; }
    }
}