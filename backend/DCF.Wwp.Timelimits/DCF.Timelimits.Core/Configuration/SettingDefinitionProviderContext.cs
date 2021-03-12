namespace DCF.Core.Configuration
{
    /// <summary>
    /// The context that is used in setting providers.
    /// </summary>
    public class SettingDefinitionProviderContext : ISettingDefinitionProviderContext
    {
        public ISettingDefinitionManager Manager { get; }

        internal SettingDefinitionProviderContext(ISettingDefinitionManager manager)
        {
            this.Manager = manager;
        }
    }
}