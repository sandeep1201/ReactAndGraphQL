using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using DCF.Common.Logging;
using DCF.Core.Configuration;

namespace DCF.Core.Configuration
{
    /// <summary>
/// Implements default behavior for ISettingStore.
/// Only <see cref="GetSettingOrNullAsync"/> method is implemented and it gets setting's value
/// from application's configuration file if exists, or returns null if not.
/// </summary>
public class DefaultConfigSettingStore : ISettingStore
    {
        /// <summary>
        /// Gets singleton instance.
        /// </summary>
        public static DefaultConfigSettingStore Instance { get { return DefaultConfigSettingStore.SingletonInstance; } }
        private static readonly DefaultConfigSettingStore SingletonInstance = new DefaultConfigSettingStore();
        private ILog _logger;

        private DefaultConfigSettingStore()
        {
            this._logger = LogProvider.GetLogger(this.GetType());
        }

        public Task<ISettingInfo> GetSettingOrNullAsync(Int64? userId, String name)
        {
            var value = ConfigurationManager.AppSettings[name];
            
            if (value == null)
            {
                return Task.FromResult<ISettingInfo>(null);
            }

            return Task.FromResult<ISettingInfo>(new SettingInfo( userId, name, value));
        }

        /// <inheritdoc/>
        public Task DeleteAsync(ISettingInfo setting)
        {
            this._logger.Warn("ISettingStore is not implemented, using DefaultConfigSettingStore which does not support DeleteAsync.");
            return Task.WhenAll();
        }

        /// <inheritdoc/>
        public Task CreateAsync(ISettingInfo setting)
        {
            this._logger.Warn("ISettingStore is not implemented, using DefaultConfigSettingStore which does not support CreateAsync.");
            return Task.WhenAll();
        }

        /// <inheritdoc/>
        public Task UpdateAsync(ISettingInfo setting)
        {
            this._logger.Warn("ISettingStore is not implemented, using DefaultConfigSettingStore which does not support UpdateAsync.");
            return Task.WhenAll();
        }

        /// <inheritdoc/>
        public Task<List<ISettingInfo>> GetAllListAsync(Int64? userId)
        {
            this._logger.Warn("ISettingStore is not implemented, using DefaultConfigSettingStore which does not support GetAllListAsync.");
            return Task.FromResult(new List<ISettingInfo>());
        }
    }
}