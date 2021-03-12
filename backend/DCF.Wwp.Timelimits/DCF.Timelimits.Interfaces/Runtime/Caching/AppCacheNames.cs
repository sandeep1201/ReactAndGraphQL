using System;

namespace DCF.Core.Runtime.Caching
{
    /// <summary>
    /// Names of standard caches used in ABP.
    /// </summary>
    public static class AppCacheNames
    {
        /// <summary>
        /// Application settings cache: AbpApplicationSettingsCache.
        /// </summary>
        public const String ApplicationSettings = "ApplicationSettingsCache";

        /// <summary>
        /// User settings cache: AbpUserSettingsCache.
        /// </summary>
        public const String UserSettings = "AbpUserSettingsCache";

    }
}