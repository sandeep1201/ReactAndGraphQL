using System;

namespace DCF.Core.Configuration
{
    /// <summary>
    /// Represents value of a setting.
    /// </summary>
    public interface ISettingValue
    {
        /// <summary>
        /// Unique name of the setting.
        /// </summary>
        String Name { get; }
        
        /// <summary>
        /// Value of the setting.
        /// </summary>
        String Value { get; }
    }
}