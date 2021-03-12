using System;
using System.Collections.Generic;

namespace DCF.Core.Configuration
{
    public interface ISettingDefinitionGroup
    {
        IReadOnlyList<ISettingDefinitionGroup> Children { get; }
        String DisplayName { get; }
        String Name { get; }
        ISettingDefinitionGroup Parent { get; set; }
        ISettingDefinitionGroup AddChild(ISettingDefinitionGroup child);
    }
}