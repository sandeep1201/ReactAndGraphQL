using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DCF.Core.Configuration
{
    /// <summary>
    /// This is the main interface that must be implemented to be able to load/change values of settings.
    /// </summary>
    public interface ISettingManager
    {
        /// <summary>
        /// Gets current value of a setting.
        /// It gets the setting value, overwritten by application, current tenant and current user if exists.
        /// </summary>
        /// <param name="name">Unique name of the setting</param>
        /// <returns>Current value of the setting</returns>
        Task<String> GetSettingValueAsync(String name);

        /// <summary>
        /// Gets current value of a setting for the application level.
        /// </summary>
        /// <param name="name">Unique name of the setting</param>
        /// <returns>Current value of the setting for the application</returns>
        Task<String> GetSettingValueForApplicationAsync(String name);

        /// <summary>
        /// Gets current value of a setting for a user level.
        /// It gets the setting value, overwritten by given tenant and user.
        /// </summary>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="tenantId">Tenant id</param>
        /// <param name="userId">User id</param>
        /// <returns>Current value of the setting for the user</returns>
        Task<String> GetSettingValueForUserAsync(String name, Int64 userId); //TODO: Can be overloaded for UserIdentifier.

        /// <summary>
        /// Gets current values of all settings.
        /// It gets all setting values, overwritten by application, current tenant (if exists) and the current user (if exists).
        /// </summary>
        /// <returns>List of setting values</returns>
        Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesAsync();

        /// <summary>
        /// Gets current values of all settings.
        /// It gets default values of all settings then overwrites by given scopes.
        /// </summary>
        /// <param name="scopes">One or more scope to overwrite</param>
        /// <returns>List of setting values</returns>
        Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesAsync(SettingScopes scopes);

        /// <summary>
        /// Gets a list of all setting values specified for the application.
        /// It returns only settings those are explicitly set for the application.
        /// If a setting's default value is used, it's not included the result list.
        /// If you want to get current values of all settings, use <see cref="GetAllSettingValuesAsync()"/> method.
        /// </summary>
        /// <returns>List of setting values</returns>
        Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesForApplicationAsync();

        /// <summary>
        /// Changes setting for the application level.
        /// </summary>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="value">Value of the setting</param>
        Task ChangeSettingForApplicationAsync(String name, String value);

        /// <summary>
        /// Changes setting for a user.
        /// </summary>
        /// <param name="user">UserId</param>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="value">Value of the setting</param>
        Task ChangeSettingForUserAsync(Int64 userId, String name, String value);
    }
}
