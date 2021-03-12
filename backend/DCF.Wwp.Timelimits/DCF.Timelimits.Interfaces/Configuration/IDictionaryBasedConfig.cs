using System;

namespace DCF.Core.Configuration
{
    /// <summary>
    /// Defines interface to use a dictionary to make configurations.
    /// </summary>
    public interface IDictionaryBasedConfig
    {
        /// <summary>
        /// Used to set a string named configuration.
        /// If there is already a configuration with same <paramref name="name"/>, it's overwritten.
        /// </summary>
        /// <param name="name">Unique name of the configuration</param>
        /// <param name="value">Value of the configuration</param>
        /// <returns>Returns the passed <paramref name="value"/></returns>
        void Set<T>(String name, T value);

        /// <summary>
        /// Gets a configuration object with given name.
        /// </summary>
        /// <param name="name">Unique name of the configuration</param>
        /// <returns>Value of the configuration or null if not found</returns>
        Object Get(String name);

        /// <summary>
        /// Gets a configuration object with given name.
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="name">Unique name of the configuration</param>
        /// <returns>Value of the configuration or null if not found</returns>
        T Get<T>(String name);

        /// <summary>
        /// Gets a configuration object with given name.
        /// </summary>
        /// <param name="name">Unique name of the configuration</param>
        /// <param name="defaultValue">Default value of the object if can not found given configuration</param>
        /// <returns>Value of the configuration or null if not found</returns>
        Object Get(String name, Object defaultValue);

        /// <summary>
        /// Gets a configuration object with given name.
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="name">Unique name of the configuration</param>
        /// <param name="defaultValue">Default value of the object if can not found given configuration</param>
        /// <returns>Value of the configuration or null if not found</returns>
        T Get<T>(String name, T defaultValue);

        /// <summary>
        /// Gets a configuration object with given name.
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="name">Unique name of the configuration</param>
        /// <param name="creator">The function that will be called to create if given configuration is not found</param>
        /// <returns>Value of the configuration or null if not found</returns>
        T GetOrCreate<T>(String name, Func<T> creator);
    }
}