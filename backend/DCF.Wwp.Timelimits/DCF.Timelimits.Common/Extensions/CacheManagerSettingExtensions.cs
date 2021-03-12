using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DCF.Core.Configuration;
using DCF.Core.Runtime.Caching;

namespace DCF.Common.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ICacheManager"/> to get setting caches.
    /// </summary>
    public static class CacheManagerSettingExtensions
    {
        /// <summary>
        /// Gets application settings cache.
        /// </summary>
        public static ITypedCache<String, Dictionary<String, ISettingInfo>> GetApplicationSettingsCache(this ICacheManager cacheManager)
        {
            return cacheManager
                .GetCache<String, Dictionary<String, ISettingInfo>>(AppCacheNames.ApplicationSettings);
        }

        /// <summary>
        /// Gets user settings cache.
        /// </summary>
        public static ITypedCache<String, Dictionary<String, ISettingInfo>> GetUserSettingsCache(this ICacheManager cacheManager)
        {
            return cacheManager
                .GetCache<String, Dictionary<String, ISettingInfo>>(AppCacheNames.UserSettings);
        }


    }
}
