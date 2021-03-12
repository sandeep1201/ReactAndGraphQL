using System;
using System.Collections.Generic;
using DCF.Common.Extensions;

namespace DCF.Core.Configuration
{
    /// <summary>
    /// Used to set/get custom configuration.
    /// </summary>
    public class DictionaryBasedConfig : IDictionaryBasedConfig
    {
        /// <summary>
        /// Dictionary of custom configuration.
        /// </summary>
        protected Dictionary<String, Object> CustomSettings { get; private set; }

        /// <summary>
        /// Gets/sets a config value.
        /// Returns null if no config with given name.
        /// </summary>
        /// <param name="name">Name of the config</param>
        /// <returns>Value of the config</returns>
        public Object this[String name]
        {
            get { return this.CustomSettings.GetOrDefault(name); }
            set { this.CustomSettings[name] = value; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected DictionaryBasedConfig()
        {
            this.CustomSettings = new Dictionary<String, Object>();
        }

        /// <summary>
        /// Gets a configuration value as a specific type.
        /// </summary>
        /// <param name="name">Name of the config</param>
        /// <typeparam name="T">Type of the config</typeparam>
        /// <returns>Value of the configuration or null if not found</returns>
        public T Get<T>(String name)
        {
            var value = this[name];
            return value == null
                ? default(T)
                : (T) Convert.ChangeType(value, typeof (T));
        }

        /// <summary>
        /// Used to set a string named configuration.
        /// If there is already a configuration with same <paramref name="name"/>, it's overwritten.
        /// </summary>
        /// <param name="name">Unique name of the configuration</param>
        /// <param name="value">Value of the configuration</param>
        public void Set<T>(String name, T value)
        {
            this[name] = value;
        }

        /// <summary>
        /// Gets a configuration object with given name.
        /// </summary>
        /// <param name="name">Unique name of the configuration</param>
        /// <returns>Value of the configuration or null if not found</returns>
        public Object Get(String name)
        {
            return this.Get(name, null);
        }

        /// <summary>
        /// Gets a configuration object with given name.
        /// </summary>
        /// <param name="name">Unique name of the configuration</param>
        /// <param name="defaultValue">Default value of the object if can not found given configuration</param>
        /// <returns>Value of the configuration or null if not found</returns>
        public Object Get(String name, Object defaultValue)
        {
            var value = this[name];
            if (value == null)
            {
                return defaultValue;
            }

            return this[name];
        }

        /// <summary>
        /// Gets a configuration object with given name.
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="name">Unique name of the configuration</param>
        /// <param name="defaultValue">Default value of the object if can not found given configuration</param>
        /// <returns>Value of the configuration or null if not found</returns>
        public T Get<T>(String name, T defaultValue)
        {
            return (T)this.Get(name, (Object)defaultValue);
        }

        /// <summary>
        /// Gets a configuration object with given name.
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="name">Unique name of the configuration</param>
        /// <param name="creator">The function that will be called to create if given configuration is not found</param>
        /// <returns>Value of the configuration or null if not found</returns>
        public T GetOrCreate<T>(String name, Func<T> creator)
        {
            var value = this.Get(name);
            if (value == null)
            {
                value = creator();
                this.Set(name, value);
            }
            return (T) value;
        }
    }
}