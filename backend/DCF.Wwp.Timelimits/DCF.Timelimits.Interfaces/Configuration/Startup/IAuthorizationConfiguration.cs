using System;
using DCF.Core.Collections;

namespace DCF.Core.Configuration.Startup
{
    /// <summary>
    /// Used to configure authorization system.
    /// </summary>
    public interface IAuthorizationConfiguration
    {
        /// <summary>
        /// List of authorization providers.
        /// </summary>
        ITypeList<IAuthorizationProvider> Providers { get; }

        /// <summary>
        /// Enables/Disables attribute based authentication and authorization.
        /// Default: true.
        /// </summary>
        Boolean IsEnabled { get; set; }
    }
}