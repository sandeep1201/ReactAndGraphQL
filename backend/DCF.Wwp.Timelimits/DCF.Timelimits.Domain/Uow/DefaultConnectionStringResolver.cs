using System;
using System.Configuration;
using DCF.Common.Exceptions;
using DCF.Core.Configuration.Startup;

namespace DCF.Core.Domain.Uow
{
    /// <summary>
    /// Default implementation of <see cref="IConnectionStringResolver"/>.
    /// Get connection string from <see cref="IStartupConfiguration"/>,
    /// or "Default" connection string in config file,
    /// or single connection string in config file.
    /// </summary>
    public class DefaultConnectionStringResolver : IConnectionStringResolver
    {
        private readonly IStartupConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultConnectionStringResolver"/> class.
        /// </summary>
        public DefaultConnectionStringResolver(IStartupConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public virtual String GetNameOrConnectionString(ConnectionStringResolveArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var defaultConnectionString = this._configuration.DefaultNameOrConnectionString;
            if (!String.IsNullOrWhiteSpace(defaultConnectionString))
            {
                return defaultConnectionString;
            }

            if (ConfigurationManager.ConnectionStrings["Default"] != null)
            {
                return "Default";
            }

            if (ConfigurationManager.ConnectionStrings.Count == 1)
            {
                return ConfigurationManager.ConnectionStrings[0].ConnectionString;
            }

            throw new DCFApplicationException("Could not find a connection string definition for the application. Set IStartupConfiguration.DefaultNameOrConnectionString or add a 'Default' connection string to application .config file.");
        }
    }
}