using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCF.Core.Modules
{
    /// <summary>
    /// Defines the state of the module,
    /// </summary>
    public enum ModuleState
    {
        /// <summary>
        /// Fully load module normally
        /// </summary>
        Enabled,
        /// <summary>
        /// Load module and dependencies normally,
        /// but do not start the modules. 
        /// Useful to debug loading module during development
        /// </summary>
        Disabled,
        /// <summary>
        /// Do not load module
        /// </summary>
        Excluded
    }
}
