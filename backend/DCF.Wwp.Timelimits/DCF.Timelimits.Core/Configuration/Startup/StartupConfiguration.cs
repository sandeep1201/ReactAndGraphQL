using System;
using System.Collections.Generic;
using DCF.Core.Auditing;
using DCF.Core.Dependency;
using DCF.Core.Domain.Uow;

namespace DCF.Core.Configuration.Startup
{
    /// <summary>
    /// This class is used to configure ABP and modules on startup.
    /// </summary>
    internal class StartupConfiguration : DictionaryBasedConfig, IStartupConfiguration
    {
        /// <summary>
        /// Reference to the IocManager.
        /// </summary>
        public IIocManager IocManager { get; }

        /// <summary>
        /// Used to configure authorization.
        /// </summary>
        public IAuthorizationConfiguration Authorization { get; private set; }

        /// <summary>
        /// Used to configure validation.
        /// </summary>
        public IValidationConfiguration Validation { get; private set; }

        /// <summary>
        /// Used to configure settings.
        /// </summary>
        public ISettingsConfiguration Settings { get; private set; }

        /// <summary>
        /// Gets/sets default connection string used by ORM module.
        /// It can be name of a connection string in application's config file or can be full connection string.
        /// </summary>
        public String DefaultNameOrConnectionString { get; set; }

        /// <summary>
        /// Used to configure modules.
        /// Modules can write extension methods to <see cref="ModuleConfigurations"/> to add module specific configurations.
        /// </summary>
        public IModuleConfigurations Modules { get; private set; }

        /// <summary>
        /// Used to configure unit of work defaults.
        /// </summary>
        public IUnitOfWorkDefaultOptions UnitOfWork { get; private set; }

        /// <summary>
        /// Used to configure notification system.
        /// </summary>
        //public INotificationConfiguration Notifications { get; private set; }

        /// <summary>
        /// Used to configure auditing.
        /// </summary>
        public IAuditingConfiguration Auditing { get; private set; }

        //public ICachingConfiguration Caching { get; private set; }


        public Dictionary<Type, Action> ServiceReplaceActions { get; private set; }

        /// <summary>
        /// Private constructor for singleton pattern.
        /// </summary>
        public StartupConfiguration(IIocManager iocManager)
        {
            this.IocManager = iocManager;
        }

        public void Initialize()
        {
            this.Modules = this.IocManager.Resolve<IModuleConfigurations>();
            this.Authorization = this.IocManager.Resolve<IAuthorizationConfiguration>();
            this.Validation = this.IocManager.Resolve<IValidationConfiguration>();
            this.Settings = this.IocManager.Resolve<ISettingsConfiguration>();
            this.UnitOfWork = this.IocManager.Resolve<IUnitOfWorkDefaultOptions>();
            this.Auditing = this.IocManager.Resolve<IAuditingConfiguration>();
            //this.Caching = this.IocManager.Resolve<ICachingConfiguration>();
            //this.Notifications = this.IocManager.Resolve<INotificationConfiguration>();
            this.ServiceReplaceActions = new Dictionary<Type, Action>();
        }

        public void ReplaceService(Type type, Action replaceAction)
        {
            this.ServiceReplaceActions[type] = replaceAction;
        }

        public T Get<T>()
        {
            return this.GetOrCreate(typeof(T).FullName, () => this.IocManager.Resolve<T>());
        }
    }
}