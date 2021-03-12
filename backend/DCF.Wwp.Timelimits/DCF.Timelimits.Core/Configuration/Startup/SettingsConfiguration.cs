using DCF.Common.Collections;
using DCF.Core.Collections;

namespace DCF.Core.Configuration.Startup
{
    internal class SettingsConfiguration : ISettingsConfiguration
    {
        public ITypeList<ISettingProvider> Providers { get; private set; }

        public SettingsConfiguration()
        {
            this.Providers = new TypeList<ISettingProvider>();
        }
    }
}