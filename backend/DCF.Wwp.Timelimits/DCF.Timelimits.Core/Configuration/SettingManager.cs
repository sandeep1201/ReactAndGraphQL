using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DCF.Common.Extensions;
using DCF.Core.Runtime.Caching;
using DCF.Core.Runtime.Session;

namespace DCF.Core.Configuration
{
    /// <summary>
    /// This class implements <see cref="ISettingManager"/> to manage setting values in the database.
    /// </summary>
    public class SettingManager : ISettingManager
    {
        public const String ApplicationSettingsCacheKey = "ApplicationSettings";

        public IApplicationSession AppSession { get; set; }

        /// <summary>
        /// Reference to the setting store.
        /// </summary>
        public ISettingStore SettingStore { get; set; }

        private readonly ISettingDefinitionManager _settingDefinitionManager;
        private readonly ITypedCache<String, Dictionary<String, ISettingInfo>> _applicationSettingCache;
        private readonly ITypedCache<Int32, Dictionary<String, ISettingInfo>> _tenantSettingCache;
        private readonly ITypedCache<String, Dictionary<String, ISettingInfo>> _userSettingCache;
        
        /// <inheritdoc/>
        public SettingManager(ISettingDefinitionManager settingDefinitionManager, ICacheManager cacheManager)
        {
            this._settingDefinitionManager = settingDefinitionManager;

            this.SettingStore = DefaultConfigSettingStore.Instance;

            this._applicationSettingCache = cacheManager.GetApplicationSettingsCache();
            this._userSettingCache = cacheManager.GetUserSettingsCache();
        }

        #region Public methods

        /// <inheritdoc/>
        public Task<String> GetSettingValueAsync(String name)
        {
            return this.GetSettingValueInternalAsync(name, this.AppSession.UserId);
        }

        public Task<String> GetSettingValueForApplicationAsync(String name)
        {
            return this.GetSettingValueInternalAsync(name);
        }

        public Task<String> GetSettingValueForUserAsync(String name, Int64 userId)
        {
            return this.GetSettingValueInternalAsync(name, userId);
        }

        public async Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesAsync()
        {
            return await this.GetAllSettingValuesAsync(SettingScopes.Application | SettingScopes.User);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesAsync(SettingScopes scopes)
        {
            var settingDefinitions = new Dictionary<String, ISettingDefinition>();
            var settingValues = new Dictionary<String, ISettingValue>();

            //Fill all setting with default values.
            foreach (var setting in this._settingDefinitionManager.GetAllSettingDefinitions())
            {
                settingDefinitions[setting.Name] = setting;
                settingValues[setting.Name] = new SettingValueObject(setting.Name, setting.DefaultValue);
            }

            //Overwrite application settings
            if (scopes.HasFlag(SettingScopes.Application))
            {
                foreach (var settingValue in await this.GetAllSettingValuesForApplicationAsync())
                {
                    var setting = settingDefinitions.GetOrDefault(settingValue.Name);

                    //TODO: Conditions get complicated, try to simplify it
                    if (setting == null || !setting.Scopes.HasFlag(SettingScopes.Application))
                    {
                        continue;
                    }

                    if (!setting.IsInherited && setting.Scopes.HasFlag(SettingScopes.User) && this.AppSession.UserId.HasValue)
                    {
                        continue;
                    }

                    settingValues[settingValue.Name] = new SettingValueObject(settingValue.Name, settingValue.Value);
                }
            }

            //Overwrite user settings
            if (scopes.HasFlag(SettingScopes.User) && this.AppSession.UserId.HasValue)
            {
                foreach (var settingValue in await GetAllSettingValuesForUserAsync(this.AppSession.UserId.Value))
                {
                    var setting = settingDefinitions.GetOrDefault(settingValue.Name);
                    if (setting != null && setting.Scopes.HasFlag(SettingScopes.User))
                    {
                        settingValues[settingValue.Name] = new SettingValueObject(settingValue.Name, settingValue.Value);
                    }
                }
            }

            return settingValues.Values.ToList().AsReadOnly();
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesForApplicationAsync()
        {
            return (await this.GetApplicationSettingsAsync()).Values
                .Select(setting => new SettingValueObject(setting.Name, setting.Value))
                .ToList().AsReadOnly();
        }


        /// <inheritdoc/>
        public async Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesForUserAsync(Int64 userId)
        {
            return (await this.GetReadOnlyUserSettings(userId)).Values
                .Select(setting => new SettingValueObject(setting.Name, setting.Value)).ToList().AsReadOnly();
        }


        /// <inheritdoc/>
        public virtual async Task ChangeSettingForApplicationAsync(String name, String value)
        {
            await this.InsertOrUpdateOrDeleteSettingValueAsync(name, value, null);
            await this._applicationSettingCache.RemoveAsync(SettingManager.ApplicationSettingsCacheKey);
        }


        /// <inheritdoc/>
        public virtual async Task ChangeSettingForUserAsync(Int64 userId, String name, String value)
        {
            await this.InsertOrUpdateOrDeleteSettingValueAsync(name, value, userId);
            await this._userSettingCache.RemoveAsync(userId.ToString());
        }

        #endregion

        #region Private methods

        private async Task<String> GetSettingValueInternalAsync(String name, Int64? userId = null)
        {
            var settingDefinition = this._settingDefinitionManager.GetSettingDefinition(name);

            //Get for user if defined
            if (settingDefinition.Scopes.HasFlag(SettingScopes.User) && userId.HasValue)
            {
                var settingValue = await this.GetSettingValueForUserOrNullAsync(userId.Value, name);
                if (settingValue != null)
                {
                    return settingValue.Value;
                }

                if (!settingDefinition.IsInherited)
                {
                    return settingDefinition.DefaultValue;
                }
            }

            //Get for application if defined
            if (settingDefinition.Scopes.HasFlag(SettingScopes.Application))
            {
                var settingValue = await this.GetSettingValueForApplicationOrNullAsync(name);
                if (settingValue != null)
                {
                    return settingValue.Value;
                }
            }

            //Not defined, get default value
            return settingDefinition.DefaultValue;
        }

        private async Task<ISettingInfo> InsertOrUpdateOrDeleteSettingValueAsync(String name, String value, Int64? userId)
        {
            var settingDefinition = this._settingDefinitionManager.GetSettingDefinition(name);
            var settingValue = await this.SettingStore.GetSettingOrNullAsync(userId, name);

            //Determine defaultValue
            var defaultValue = settingDefinition.DefaultValue;

            if (settingDefinition.IsInherited)
            {
                //For User, Application's value overrides Setting Definition's default value.
                if (userId.HasValue)
                {
                    var applicationValue = await this.GetSettingValueForApplicationOrNullAsync(name);
                    if (applicationValue != null)
                    {
                        defaultValue = applicationValue.Value;
                    }
                }
            }

            //No need to store on database if the value is the default value
            if (value == defaultValue)
            {
                if (settingValue != null)
                {
                    await this.SettingStore.DeleteAsync(settingValue);
                }

                return null;
            }

            //If it's not default value and not stored on database, then insert it
            if (settingValue == null)
            {
                settingValue = new SettingInfo
                {
                    UserId = userId,
                    Name = name,
                    Value = value
                };

                await this.SettingStore.CreateAsync(settingValue);
                return settingValue;
            }

            //It's same value in database, no need to update
            if (settingValue.Value == value)
            {
                return settingValue;
            }

            //Update the setting on database.
            settingValue.Value = value;
            await this.SettingStore.UpdateAsync(settingValue);

            return settingValue;
        }

        private async Task<ISettingInfo> GetSettingValueForApplicationOrNullAsync(String name)
        {
            return (await this.GetApplicationSettingsAsync()).GetOrDefault(name);
        }


        private async Task<ISettingInfo> GetSettingValueForUserOrNullAsync(Int64 userId, String name)
        {
            return (await this.GetReadOnlyUserSettings(userId)).GetOrDefault(name);
        }

        private async Task<Dictionary<String, ISettingInfo>> GetApplicationSettingsAsync()
        {
            return await this._applicationSettingCache.GetAsync(SettingManager.ApplicationSettingsCacheKey, async () =>
            {
                var dictionary = new Dictionary<String, ISettingInfo>();

                var settingValues = await this.SettingStore.GetAllListAsync(null);
                foreach (var settingValue in settingValues)
                {
                    dictionary[settingValue.Name] = settingValue;
                }

                return dictionary;
            });
        }

        private async Task<ReadOnlyDictionary<String, ISettingInfo>> GetReadOnlyUserSettings(Int64 userId)
        {
            var cachedDictionary = await this.GetUserSettingsFromCache(userId);
            lock (cachedDictionary)
            {
                return cachedDictionary.ToReadOnlyDictionary();
            }
        }


        private async Task<Dictionary<String, ISettingInfo>> GetUserSettingsFromCache(Int64 userId)
        {
            return await this._userSettingCache.GetAsync(
                userId.ToString(),
                async () =>
                {
                    var dictionary = new Dictionary<String, ISettingInfo>();

                    var settingValues = await this.SettingStore.GetAllListAsync(userId);
                    foreach (var settingValue in settingValues)
                    {
                        dictionary[settingValue.Name] = settingValue;
                    }

                    return dictionary;
                });
        }

        #endregion

        #region Nested classes

        private class SettingValueObject : ISettingValue
        {
            public String Name { get; private set; }

            public String Value { get; private set; }

            public SettingValueObject(String name, String value)
            {
                this.Value = value;
                this.Name = name;
            }
        }

        #endregion
    }
}