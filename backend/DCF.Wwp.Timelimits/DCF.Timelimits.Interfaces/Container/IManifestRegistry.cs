using System.Collections.Generic;
using DCF.Core.Plugins;

namespace DCF.Core.Container
{
    public interface IManifestRegistry
    {
        List<IPluginManifest> AllManifests { get; set; }
        IEnumerable<IAppComponentPluginManifest> AppComponentPluginManifests { get; }
        IEnumerable<IAppHostPluginManifest> AppHostPluginManifests { get; }
        IAppHostPluginManifest AppPluginManifest { get; }
        IEnumerable<ICorePluginManifest> CorePluginManifests { get; }
    }
}