using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DCF.Common.Logging;
using Newtonsoft.Json.Linq;

namespace DCF.Core.Configuration
{
    /// <summary>
    /// Implements default behavior for ISettingStore.
    /// Only <see cref="GetSettingOrNullAsync"/> method is implemented and it gets setting's value
    /// from application's configuration file if exists, or returns null if not.
    /// </summary>
    public class JsonConfigFileSettingStore : ISettingStore
    {
        private readonly String _filename;
        private ILog _logger;
        private Lazy<JObject> _configObject;

        public String ConfigName => Path.GetFileNameWithoutExtension(this._filename);


        private JsonConfigFileSettingStore(string filename)
        {
            this._filename = filename;
            //Lazily Load the configuration from the file
            this._configObject = new Lazy<JObject>(() =>
            {
                try
                {
                    return JObject.Parse(File.ReadAllText(this._filename));
                }
                catch (Exception e)
                {
                    this._logger.ErrorException("Error loading json config file: {0}", e, filename);
                }
                return new JObject();
            });
            this._logger = LogProvider.GetLogger(this.GetType());
        }

        public Task<ISettingInfo> GetSettingOrNullAsync(Int64? userId, String name)
        {
            var value = this._configObject.Value.SelectToken(name);

            if (value == null)
            {
                return Task.FromResult<ISettingInfo>(null);
            }

            return Task.FromResult<ISettingInfo>(new SettingInfo(userId, name, value.Value<String>()));
        }

        /// <inheritdoc/>
        public Task DeleteAsync(ISettingInfo setting)
        {
            //this._configObject.Value.
            this._logger.Warn("ISettingStore is not implemented, using DefaultConfigSettingStore which does not support CreateAsync.");
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