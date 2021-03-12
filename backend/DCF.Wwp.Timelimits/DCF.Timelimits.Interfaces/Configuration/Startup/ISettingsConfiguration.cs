using DCF.Core.Collections;

namespace DCF.Core.Configuration.Startup
{
    /// <summary>
    /// Used to configure setting system.
    /// </summary>
    public interface ISettingsConfiguration
    {
        /// <summary>
        /// List of settings providers.
        /// </summary>
        ITypeList<ISettingProvider> Providers { get; }
    }
}
