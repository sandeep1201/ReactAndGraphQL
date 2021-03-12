using System.Collections.Generic;

namespace DCF.Core.Configuration
{
    /// <summary>
    /// Inherit this class to define settings for a module/application.
    /// </summary>
    public abstract class SettingProvider : ISettingProvider
    {
        /// <summary>
        /// Gets all setting definitions provided by this provider.
        /// </summary>
        /// <returns>List of settings</returns>
        public abstract IEnumerable<ISettingDefinition> GetSettingDefinitions(ISettingDefinitionProviderContext context);
    }
}