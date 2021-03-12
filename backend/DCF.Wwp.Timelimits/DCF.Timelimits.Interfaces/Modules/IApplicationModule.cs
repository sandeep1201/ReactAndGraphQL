using System;
using System.Collections.Generic;
using Castle.Windsor;
using DCF.Core.Plugins;

namespace DCF.Core.Modules
{
    public interface IApplicationModule
    {
        String AssemblyName { get; }
        IModuleContext Context { get; set; }
        List<IApplicationModule> Dependencies { get; }
        IPluginManifest Manifest { get; }
        ModuleState ModuleState { get; set; }

        void Initialize();
        void PostInitialize();
        void PreInitialize();
        void Configure();

        /// <summary>
        /// Called first for all plug-in modules to allow for default instances
        /// of services to be registered to be used if not overridden by another
        /// plug-in module.
        /// </summary>
        /// <param name="container"></param>
        void RegisterDefaultComponents(IWindsorContainer container);

        /// <summary>
        /// Allows the plug-in to scan for types it defines that are 
        /// to be registered with the dependency injection container.
        /// Scans only contains types contained in the plug-in associated with
        /// the module.</summary>
        /// <param name="builder"></param>
        void ScanPlugin(ITypeRegistration builder);

        /// <summary>
        /// Allows the plug-in to register specific components
        /// as services within the dependency injection container.
        /// </summary>
        /// <param name="container"></param>
        void RegisterComponents(IWindsorContainer container);

        /// <summary>
        /// Allows the plug-in module to scan for types within all other plug-ins.
        /// This registration contains all other plug-in types when called on a
        /// core plug in.  For an application plug-in, the types are limited to 
        /// only other application plug ins.
        /// </summary>
        /// <param name="typeRegistration"></param>
        void ScanAllOtherPlugins(ITypeRegistration typeRegistration);

        /// <summary>
        /// Allows a core plug-in module to scan for type limited to only 
        /// application centric plug-in types.
        /// </summary>
        void ScanApplicationPlugins(ITypeRegistration typeRegistration);

        /// <summary>
        /// The last method called on the module by the bootstrap process.  
        /// Called after all types have been registered and the container
        /// has been created.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="scope">Child scope of the created container.</param>
        void Start(IWindsorContainer container);

        /// <summary>
        /// Called after all modules have been started.
        /// </summary>
        /// <param name="container"></param>
        void Run(IWindsorContainer container);

        /// <summary>
        /// Called after the module is initialized and configured so
        /// that it can add module specific logs to the application
        /// composite log.
        /// </summary>
        /// <param name="moduleLog">Log dictionary to populate.</param>
        void Log(IDictionary<String, Object> moduleLog);

        /// <summary>
        /// Called when the container is stopped.  Allows the module to
        /// complete any processing before the container is stopped.
        /// </summary>
        /// <param name="container"></param>
        void Shutdown();
    }
}