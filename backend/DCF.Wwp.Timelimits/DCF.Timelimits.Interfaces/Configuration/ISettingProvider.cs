using System.Collections.Generic;

namespace DCF.Core.Configuration
{
    public interface ISettingProvider
    {
        IEnumerable<ISettingDefinition> GetSettingDefinitions(ISettingDefinitionProviderContext context);
    }
}