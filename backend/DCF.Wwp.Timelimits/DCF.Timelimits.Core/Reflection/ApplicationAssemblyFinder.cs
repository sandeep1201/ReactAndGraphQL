using DCF.Core.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCF.Core.Reflection
{
    public class ApplicationAssemblyFinder
    {
        private readonly IApplicationModuleManager _moduleManager;

        public ApplicationAssemblyFinder(IApplicationModuleManager moduleManager)
        {
            this._moduleManager = moduleManager;
        }


    }
}
