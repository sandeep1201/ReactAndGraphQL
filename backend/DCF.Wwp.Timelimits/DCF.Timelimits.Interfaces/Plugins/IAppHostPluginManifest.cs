namespace DCF.Core.Plugins
{
    /// <summary>
    /// Used to define the host application module (web-api,console, other runtime).
    /// There should only be one of these
    /// </summary>
    public interface IAppHostPluginManifest : IPluginManifest
    {
    }
}