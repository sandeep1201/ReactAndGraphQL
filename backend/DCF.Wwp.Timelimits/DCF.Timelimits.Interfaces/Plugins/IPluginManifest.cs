using System;

namespace DCF.Core.Plugins
{
    /// <summary>
    /// Base interface use to indentifty assemblies that contain modules/plugins to be bootstrapped
    /// TODO: It would be nice to use conventions to generate internal manifests like Durandal does
    /// </summary>
    public interface IPluginManifest
    {
        String PluginId {get; }
        String Name {get; } 
        String AssemblyName {get;set;}
        String AssemblyVersion {get;set; }
        String MachineName {get;set; }
        String Description {get;set; }
    }
}