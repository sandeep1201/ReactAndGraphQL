using System;
using System.Threading.Tasks;

namespace DCF.Core.Runtime.Caching
{
    /// <summary>
    /// Defines a cache that can be store and get items by keys.
    /// </summary>
    public interface ICache : IDisposable
    {
        /// <summary>
        /// Unique name of the cache.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Default sliding expire time of cache items.
        /// Default value: 60 minutes (1 hour).
        /// Can be changed by configuration.
        /// </summary>
        TimeSpan DefaultSlidingExpireTime { get; set; }

        /// <summary>
        /// Default absolute expire time of cache items.
        /// Default value: null (not used).
        /// </summary>
        TimeSpan? DefaultAbsoluteExpireTime { get; set; }

        /// <summary>
        /// Gets an item from the cache.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="factory">Factory method to create cache item if not exists</param>
        /// <returns>Cached item</returns>
        Object Get(String key, Func<String, Object> factory);

        /// <summary>
        /// Gets an item from the cache.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="factory">Factory method to create cache item if not exists</param>
        /// <returns>Cached item</returns>
        Task<Object> GetAsync(String key, Func<String, Task<Object>> factory);

        /// <summary>
        /// Gets an item from the cache or null if not found.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Cached item or null if not found</returns>
        Object GetOrDefault(String key);

        /// <summary>
        /// Gets an item from the cache or null if not found.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Cached item or null if not found</returns>
        Task<Object> GetOrDefaultAsync(String key);

        /// <summary>
        /// Saves/Overrides an item in the cache by a key.
        /// Use one of the expire times at most (<see cref="slidingExpireTime"/> or <see cref="absoluteExpireTime"/>).
        /// If none of them is specified, then
        /// <see cref="DefaultAbsoluteExpireTime"/> will be used if it's not null. Othewise, <see cref="DefaultSlidingExpireTime"/>
        /// will be used.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="slidingExpireTime">Sliding expire time</param>
        /// <param name="absoluteExpireTime">Absolute expire time</param>
        void Set(String key, Object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        /// <summary>
        /// Saves/Overrides an item in the cache by a key.
        /// Use one of the expire times at most (<see cref="slidingExpireTime"/> or <see cref="absoluteExpireTime"/>).
        /// If none of them is specified, then
        /// <see cref="DefaultAbsoluteExpireTime"/> will be used if it's not null. Othewise, <see cref="DefaultSlidingExpireTime"/>
        /// will be used.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="slidingExpireTime">Sliding expire time</param>
        /// <param name="absoluteExpireTime">Absolute expire time</param>
        Task SetAsync(String key, Object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        /// <summary>
        /// Removes a cache item by it's key.
        /// </summary>
        /// <param name="key">Key</param>
        void Remove(String key);

        /// <summary>
        /// Removes a cache item by it's key (does nothing if given key does not exists in the cache).
        /// </summary>
        /// <param name="key">Key</param>
        Task RemoveAsync(String key);

        /// <summary>
        /// Clears all items in this cache.
        /// </summary>
        void Clear();

        /// <summary>
        /// Clears all items in this cache.
        /// </summary>
        Task ClearAsync();
    }
}