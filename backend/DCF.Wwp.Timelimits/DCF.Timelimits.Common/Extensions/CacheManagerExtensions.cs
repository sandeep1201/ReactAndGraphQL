using System;
using DCF.Core.Runtime.Caching;

namespace DCF.Common.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ICacheManager"/>.
    /// </summary>
    public static class CacheManagerExtensions
    {
        public static ITypedCache<TKey, TValue> GetCache<TKey, TValue>(this ICacheManager cacheManager, String name)
        {
            return cacheManager.GetCache(name).AsTyped<TKey, TValue>();
        }
    }
}