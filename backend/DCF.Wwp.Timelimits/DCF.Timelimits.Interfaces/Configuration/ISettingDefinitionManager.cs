using System;
using System.Collections.Generic;

namespace DCF.Core.Configuration
{
    /// <summary>
    /// Defines setting definition manager.
    /// </summary>
    public interface ISettingDefinitionManager
    {
        /// <summary>
        /// Gets the <see cref="SettingDefinition"/> object with given unique name.
        /// Throws exception if can not find the setting.
        /// </summary>
        /// <param name="name">Unique name of the setting</param>
        /// <returns>The <see cref="SettingDefinition"/> object.</returns>
        ISettingDefinition GetSettingDefinition(String name);

        /// <summary>
        /// Gets a list of all setting definitions.
        /// </summary>
        /// <returns>All settings.</returns>
        IReadOnlyList<ISettingDefinition> GetAllSettingDefinitions();
    }
}
