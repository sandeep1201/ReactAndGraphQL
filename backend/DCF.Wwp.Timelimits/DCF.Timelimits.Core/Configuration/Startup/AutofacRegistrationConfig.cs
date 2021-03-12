using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;

namespace DCF.Core.Configuration.Startup
{
    /// <summary>
    /// Configuration class used by the host application to specify an action delegate
    /// that is called and passed an instance of the Windosor container.  This
    /// allows the host application to register any types and/or object instances with
    /// the container before the application plugins are bootstrapped (i.e. logger instance).
    /// </summary>
    public class AutofacRegistrationConfig
    {
        /// <summary>
        /// Action delegate passed instance of the builder before
        /// the container is created.
        /// </summary>
        public Action<IWindsorContainer> Build { get; set; }
    }
}
