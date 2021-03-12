namespace DCF.Core.Configuration
{
    public interface ISettingDefinitionProviderContext
    {
        ISettingDefinitionManager Manager { get; }
    }
}