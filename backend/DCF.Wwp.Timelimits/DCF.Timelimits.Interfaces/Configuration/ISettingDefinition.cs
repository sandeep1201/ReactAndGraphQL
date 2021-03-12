using System;

namespace DCF.Core.Configuration
{
    public interface ISettingDefinition
    {
        Object CustomData { get; set; }
        String DefaultValue { get; set; }
        String Description { get; set; }
        String DisplayName { get; set; }
        ISettingDefinitionGroup Group { get; set; }
        Boolean IsInherited { get; set; }
        Boolean IsVisibleToClients { get; set; }
        String Name { get; }
        SettingScopes Scopes { get; set; }
    }
}