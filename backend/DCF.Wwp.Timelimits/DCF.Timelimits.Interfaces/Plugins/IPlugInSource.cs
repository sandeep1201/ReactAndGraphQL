using DCF.Core.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCF.Core.Plugins
{
    public interface IPlugInSource
    {
        List<Type> GetModules();
    }
}
