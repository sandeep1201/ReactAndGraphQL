using System;
using System.Collections.Generic;
using System.Linq;
using DCF.Common.Exceptions;
using DCF.Common.Extensions;
using DCF.Core.Configuration.Startup;
using DCF.Core.Dependency;

namespace DCF.Core.Configuration
{
    /// <summary>
    /// Implements <see cref="ISettingDefinitionManager"/>.
    /// </summary>
    internal class SettingDefinitionManager : ISettingDefinitionManager
    {
        private readonly IIocManager _iocManager;
        private readonly ISettingsConfiguration _settingsConfiguration;
        private readonly IDictionary<String, ISettingDefinition> _settings;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SettingDefinitionManager(IIocManager iocManager, ISettingsConfiguration settingsConfiguration)
        {
            this._iocManager = iocManager;
            this._settingsConfiguration = settingsConfiguration;
            this._settings = new Dictionary<String, ISettingDefinition>();
        }

        public void Initialize()
        {
            var context = new SettingDefinitionProviderContext(this);

            foreach (var providerType in this._settingsConfiguration.Providers)
            {
                using (var provider = this.CreateProvider(providerType))
                {
                    foreach (var settings in provider.Object.GetSettingDefinitions(context))
                    {
                        this._settings[settings.Name] = settings;
                    }
                }
            }
        }

        public ISettingDefinition GetSettingDefinition(String name)
        {
            ISettingDefinition settingDefinition;
            if (!this._settings.TryGetValue(name, out settingDefinition))
            {
                throw new DCFApplicationException("There is no setting defined with name: " + name);
            }

            return settingDefinition;
        }

        public IReadOnlyList<ISettingDefinition> GetAllSettingDefinitions()
        {
            return this._settings.Values.ToList().AsReadOnly();
        }

        private IDisposableDependencyObjectWrapper<SettingProvider> CreateProvider(Type providerType)
        {
            this._iocManager.RegisterIfNot(providerType, DependencyLifeStyle.Transient); //TODO: Needed?
            return this._iocManager.ResolveAsDisposable<SettingProvider>(providerType);
        }
    }
}