using System;
using System.Collections.Generic;
using System.Reflection;
using DCF.Core.Plugins;

namespace DCF.Core.Modules
{
    public interface IApplicationModuleInfo
    {
        Assembly Assembly { get; }
        IList<IApplicationModuleInfo> Dependencies { get; }
        IEnumerable<IApplicationModule> DiscoveredByModules { get; set; }
        IApplicationModule Instance { get; }
        IPluginManifest Manifest { get; set; }
        Type Type { get; }

    }
}