using Castle.MicroKernel.Registration;
using DCF.Core.Modules;
using DCF.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCF.Core.Dependency
{
    public class ApplicationCoreInstaller : Castle.MicroKernel.Registration.IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            //TODO
            container.Register(
                    Component.For<IApplicationPluginManager, ApplicationPluginManager>().LifestyleSingleton(),
                    Component.For<IApplicationModuleManager, ApplicationModuleManager>().LifestyleSingleton()
                );
        }

        #endregion
    }
}
