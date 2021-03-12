using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DCF.Core.Configuration
{
    /// <summary>
    /// This interface is used to get/set settings from/to a data source (database).
    /// </summary>
    public interface ISettingStore
    {
        /// <summary>
        /// Gets a setting or null.
        /// </summary>
        /// <param name="userId">UserId or null</param>
        /// <param name="name">Name of the setting</param>
        /// <returns>Setting object</returns>
        Task<ISettingInfo> GetSettingOrNullAsync(Int64? userId, String name);

        /// <summary>
        /// Deletes a setting.
        /// </summary>
        /// <param name="setting">Setting to be deleted</param>
        Task DeleteAsync(ISettingInfo setting);

        /// <summary>
        /// Adds a setting.
        /// </summary>
        /// <param name="setting">Setting to add</param>
        Task CreateAsync(ISettingInfo setting);

        /// <summary>
        /// Update a setting.
        /// </summary>
        /// <param name="setting">Setting to add</param>
        Task UpdateAsync(ISettingInfo setting);

        /// <summary>
        /// Gets a list of setting.
        /// </summary>
        /// <param name="userId">UserId or null</param>
        /// <returns>List of settings</returns>
        Task<List<ISettingInfo>> GetAllListAsync(Int64? userId);
    }
}