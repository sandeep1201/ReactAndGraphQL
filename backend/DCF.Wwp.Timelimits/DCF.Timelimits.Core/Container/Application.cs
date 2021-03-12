using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Lifestyle.Scoped;
using Castle.Windsor;
using DCF.Core.Dependency;
using DCF.Core.Logging;
using DCF.Core.Modules;
using DCF.Core.Plugins;
using EnsureThat;

namespace DCF.Core.Container
{
    /// <summary>
    /// Represents an application that is composed of plug-ins that are used
    /// to create a run-time environment and dependency injection container.
    /// </summary>
    public class Application
    {
        public Boolean IsStarted { get; private set; }
        public PlugInSourceList PluginSources { get; }

        internal ILog Logger { get; set; }

        public Application(PlugInSourceList pluginSources)
        {
            Ensure.That(pluginSources, nameof(pluginSources)).IsNotNull();

            this.PluginSources = pluginSources;
        }

        public Plugin[] Plugins { get; internal set; }

        public Plugin AppHostPlugin
        {
            get { return this.Plugins.First(p => p.Manifest is IAppHostPluginManifest); }
        }

        public IEnumerable<Plugin> AppComponentPlugins
        {
            get { return this.Plugins.Where(p => p.Manifest is IAppComponentPluginManifest); }
        }

        public IEnumerable<Plugin> CorePlugins
        {
            get { return this.Plugins.Where(p => p.Manifest is ICorePluginManifest); }
        }

        /// <summary>
        /// Returns types associated with a specific category of plug-in.
        /// </summary>
        /// <param name="pluginTypes">The category of plug-ins to limit the return types.</param>
        /// <returns>List of limited plug in types or all plug-in types if no category is specified.</returns>
        public IEnumerable<IPluginType> GetPluginTypesFrom(params PluginTypes[] pluginTypes)
        {
            Ensure.That(pluginTypes, nameof(pluginTypes)).IsNotNull();

            if (pluginTypes.Length == 0)
            {
                return this.Plugins.SelectMany(p => p.PluginTypes);
            }

            return this.Plugins.SelectMany(p => p.PluginTypes)
                .Where(pt => pluginTypes.Contains(pt.Plugin.PluginType));
        }

        public IEnumerable<IApplicationModule> AllAppModules
        {
            get { return this.Plugins?.SelectMany(p => p.AppModules); }
        }

        /// <summary>
        /// Populates the dependency injection container with services
        /// registered by plug-in modules.
        /// </summary>
        /// <param name="applicationKernal">The DI container.</param>
        public void RegisterComponents(IIocManager iocManager)
        {
            this.IocManager = iocManager;
            Ensure.That(iocManager, nameof(iocManager)).IsNotNull();

            this.InitializePluginModules();

            // Note that the order is important.  In Windsor, if a service type 
            // is registered more than once, the first registered component is
            // used.  This is the default configuration.
            this.RegisterDefaultPluginComponents(iocManager.IocContainer);
            this.RegisterCorePluginComponents(iocManager.IocContainer);
            this.RegisterAppPluginComponents(iocManager.IocContainer);
        }

        public IIocManager IocManager { get; set; }

        private void InitializePluginModules()
        {
            this.InitializePluginModules(this.CorePlugins);
            this.InitializePluginModules(this.AppComponentPlugins);
            this.InitializePluginModules(new[] { this.AppHostPlugin });
        }

        private void InitializePluginModules(IEnumerable<Plugin> plugins)
        {
            foreach (var plugin in plugins)
            {
                foreach (var module in plugin.IncludedModules())
                {
                    module.PreInitialize();
                }

                foreach (var module in plugin.IncludedModules())
                {
                    module.Context = new ModuleContext(this, plugin);
                    module.Initialize();
                }

                foreach (var module in plugin.IncludedModules())
                {
                    module.PostInitialize();
                }

                foreach (var module in plugin.IncludedModules())
                {
                    module.Configure();
                }
            }
        }

        // First allow all plug-ins to register any default component implementations
        // that can be optionally overridden by other plug-ins.  This will be the
        // component instances that will be used if not overridden.  
        // Null implementations can also be specified.
        private void RegisterDefaultPluginComponents(IWindsorContainer container)
        {
            foreach (var module in this.AllAppModules)
            {
                module.RegisterDefaultComponents(container);
            }
        }

        private void RegisterCorePluginComponents(IWindsorContainer container)
        {
            var allPluginTypes = this.GetPluginTypesFrom().ToList();
            foreach (var plugin in this.CorePlugins)
            {
                this.ScanPluginTypes(plugin, container);
                this.RegisterComponents(plugin, container);

                // Core modules may override one or both of the following depending 
                // on the scope of the search.
                this.ScanAllOtherPluginTypes(plugin, container, allPluginTypes);
                this.ScanOnlyApplicationPluginTypes(plugin,container);
            }
        }

        // Allows for each plug-in module to scan its types for any
        // service components to be registered in the IocManager.
        private void ScanPluginTypes(Plugin plugin, IWindsorContainer container)
        {
            var typeRegistration = new TypeRegistration(container, plugin.PluginTypes);
            foreach (var module in plugin.IncludedModules())
            {
                module.ScanPlugin(typeRegistration);
            }
        }

        // Allows the each plug-in module to manually register
        // any needed service components with the Autofac IocManager.
        private void RegisterComponents(IPlugin plugin, IWindsorContainer container)
        {
            foreach (var module in plugin.IncludedModules())
            {
                module.RegisterComponents(container);
            }
        }

        // Allows a plug-in to scan all specified plug-in types, excluding types
        // defined within it's plug-in, for components to be registered in the
        // Autofac container.
        private void ScanAllOtherPluginTypes(Plugin plugin,
            IWindsorContainer container,
            IEnumerable<IPluginType> sourceTypes)
        {
            var typeRegistration = new TypeRegistration(
                container,
                sourceTypes.Except(plugin.PluginTypes));



            foreach (var module in plugin.IncludedModules())
            {
                module.ScanAllOtherPlugins(typeRegistration);
            }
        }

        private void ScanOnlyApplicationPluginTypes(Plugin plugin,
            IWindsorContainer container)
        {
            var appPluginTypes = this.GetPluginTypesFrom(PluginTypes.AppComponentPlugin, PluginTypes.AppHostPlugin);

            var typeRegistration = new TypeRegistration(
                container,
                appPluginTypes);

            foreach (var module in plugin.IncludedModules())
            {
                module.ScanApplicationPlugins(typeRegistration);
            }
        }

        private void RegisterAppPluginComponents(IWindsorContainer container)
        {
            var allAppPluginTypes = this.GetPluginTypesFrom(PluginTypes.AppComponentPlugin, PluginTypes.AppHostPlugin);

            // Application Components:
            var appPluginTypes = allAppPluginTypes as IPluginType[] ?? allAppPluginTypes.ToArray();
            foreach (var plugin in this.AppComponentPlugins)
            {
                this.ScanPluginTypes(plugin, container);
                this.RegisterComponents(plugin, container);
                this.ScanAllOtherPluginTypes(plugin, container, appPluginTypes);
            }

            // Application Host:
            this.ScanPluginTypes(this.AppHostPlugin, container);
            this.RegisterComponents(this.AppHostPlugin, container);
            this.ScanAllOtherPluginTypes(this.AppHostPlugin, container, appPluginTypes);
        }

        /// <summary>
        /// This is the last step of the bootstrap process.  Each module
        /// is passed the instance of the created container so that it
        /// can enable any runtime services requiring the container.
        /// </summary>
        /// <param name="container">The built container.</param>
        public void StartPluginModules(IWindsorContainer container)
        {
            Ensure.That(container, nameof(container)).IsNotNull();

            // Start the plug-in modules in dependent order starting with core modules 
            // and ending with the application host modules.
            this.IsStarted = true;

            using (container.BeginScope())
            {
                this.StartPluginModules(container, this.CorePlugins);
                this.StartPluginModules(container, this.AppComponentPlugins);
                this.StartPluginModules(container, new[] { this.AppHostPlugin });

                // Last phase to allow any modules to execute an processing that
                // might be dependent on another module being started.
                foreach (var module in this.Plugins.SelectMany(p => p.IncludedModules()))
                {
                    module.Run(container);
                }
            }
        }

        private void StartPluginModules(IWindsorContainer container, IEnumerable<Plugin> plugins)
        {
            foreach (var module in plugins.SelectMany(p => p.IncludedModules()))
            {
                module.Start(container);
            }
        }

        /// <summary>
        /// Stops all plug-in modules in the reverse order from which they were started.
        /// </summary>
        /// <param name="container">The build container.</param>
        public void StopPluginModules(IWindsorContainer container)
        {
            using (container.BeginScope())
            {
                this.StopPluginModules(container, new[] { this.AppHostPlugin });
                this.StopPluginModules(container, this.AppComponentPlugins);
                this.StopPluginModules(container, this.CorePlugins);
            }

            this.IsStarted = false;
        }

        private void StopPluginModules(IWindsorContainer container, IEnumerable<Plugin> plugins)
        {
            foreach (var module in plugins.SelectMany(p => p.IncludedModules()))
            {
                module.Shutdown();
            }
        }
    }
}
