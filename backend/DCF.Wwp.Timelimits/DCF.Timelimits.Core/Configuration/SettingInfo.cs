using System;

namespace DCF.Core.Configuration
{
    /// <summary>
    /// Represents a setting information.
    /// </summary>
    [Serializable]
    public class SettingInfo : ISettingInfo
    {
        /// <summary>
        /// UserId for this setting.
        /// UserId is null if this setting is not user level.
        /// </summary>
        public Int64? UserId { get; set; }

        /// <summary>
        /// Unique name of the setting.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Value of the setting.
        /// </summary>
        public String Value { get; set; }

        /// <summary>
        /// Creates a new <see cref="SettingInfo"/> object.
        /// </summary>
        public SettingInfo()
        {
            
        }

        /// <summary>
        /// Creates a new <see cref="SettingInfo"/> object.
        /// </summary>
        /// <param name="userId">UserId for this setting. UserId is null if this setting is not user level.</param>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="value">Value of the setting</param>
        public SettingInfo(Int64? userId, String name, String value)
        {
            this.UserId = userId;
            this.Name = name;
            this.Value = value;
        }
    }
}