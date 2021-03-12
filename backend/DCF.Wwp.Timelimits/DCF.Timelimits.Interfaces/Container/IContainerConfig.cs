using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCF.Core.Plugins;

namespace DCF.Core.Container
{
    /// <summary>
    /// Allows plug-ins to define specific configurations that can
    /// be initialized by the host application to alter the behavior
    /// of the plug-in.
    /// </summary>
    public interface IContainerConfig : IKnownPluginType
    {
        
    }
}
