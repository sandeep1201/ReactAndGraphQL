using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCF.Core.Logging;
using DCF.Core.Modules;
using DCF.Core.Plugins;
using EnsureThat;

namespace DCF.Core.Runtime
{
    public class CompositeApplication
    {
        public Boolean IsStarted {get; private set;}

        internal ILog Logger { get; set; }

        public ApplicationModuleCollection ApplicationModules { get; set; }

        public CompositeApplication(PlugInSourceList pluginSources) {

        }


        public ApplicationModule AppHostModule
        {
            get { return this.ApplicationModules?.First(p => p.Manifest is IAppHostModuleManifest); }
        }
        public IEnumerable<ApplicationModule> AppComponentPlugins
        {
            get { return this.ApplicationModules.Where(p => p.Manifest is IAppComponentModuleManifest); }
        }

        public IEnumerable<ApplicationModule> CorePlugins
        {
            get { return this.ApplicationModules.Where(p => p.Manifest is ICoreModuleManifest); }
        }


    }
}
