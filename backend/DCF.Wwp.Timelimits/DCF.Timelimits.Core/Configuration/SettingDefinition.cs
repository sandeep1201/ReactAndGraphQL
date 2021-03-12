using System;

namespace DCF.Core.Configuration
{
    /// <summary>
    /// Defines a setting.
    /// A setting is used to configure and change behavior of the application.
    /// </summary>
    public class SettingDefinition : ISettingDefinition
    {
        /// <summary>
        /// Unique name of the setting.
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// Display name of the setting.
        /// This can be used to show setting to the user.
        /// </summary>
        public String DisplayName { get; set; }

        /// <summary>
        /// A brief description for this setting.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Scopes of this setting.
        /// Default value: <see cref="SettingScopes.Application"/>.
        /// </summary>
        public SettingScopes Scopes { get; set; }

        /// <summary>
        /// Is this setting inherited from parent scopes.
        /// Default: True.
        /// </summary>
        public Boolean IsInherited { get; set; }

        /// <summary>
        /// Gets/sets group for this setting.
        /// </summary>
        public ISettingDefinitionGroup Group { get; set; }

        /// <summary>
        /// Default value of the setting.
        /// </summary>
        public String DefaultValue { get; set; }

        /// <summary>
        /// Can clients see this setting and it's value.
        /// It maybe dangerous for some settings to be visible to clients (such as email server password).
        /// Default: false.
        /// </summary>
        public Boolean IsVisibleToClients { get; set; }

        /// <summary>
        /// Can be used to store a custom object related to this setting.
        /// </summary>
        public Object CustomData { get; set; }

        /// <summary>
        /// Creates a new <see cref="SettingDefinition"/> object.
        /// </summary>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="defaultValue">Default value of the setting</param>
        /// <param name="displayName">Display name of the permission</param>
        /// <param name="group">Group of this setting</param>
        /// <param name="description">A brief description for this setting</param>
        /// <param name="scopes">Scopes of this setting. Default value: <see cref="SettingScopes.Application"/>.</param>
        /// <param name="isVisibleToClients">Can clients see this setting and it's value. Default: false</param>
        /// <param name="isInherited">Is this setting inherited from parent scopes. Default: True.</param>
        /// <param name="customData">Can be used to store a custom object related to this setting</param>
        public SettingDefinition(
            String name, 
            String defaultValue, 
            String displayName = null, 
            SettingDefinitionGroup group = null, 
            String description = null, 
            SettingScopes scopes = SettingScopes.Application, 
            Boolean isVisibleToClients = false, 
            Boolean isInherited = true,
            Object customData = null)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
            this.DefaultValue = defaultValue;
            this.DisplayName = displayName;
            this.Group = @group;
            this.Description = description;
            this.Scopes = scopes;
            this.IsVisibleToClients = isVisibleToClients;
            this.IsInherited = isInherited;
            this.CustomData = customData;
        }
    }
}
