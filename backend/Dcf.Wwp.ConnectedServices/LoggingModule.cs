using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Dcf.Wwp.ConnectedServices.Logging;
using log4net;

namespace Dcf.Wwp.ConnectedServices
{
    public class LoggingModule : Autofac.Module
    {
        #region Properties
        
        public string AppName { get; set; }

        #endregion

        #region Methods

        public LoggingModule() : this(AppDomain.CurrentDomain.FriendlyName)
        {
            AppName = AppDomain.CurrentDomain.FriendlyName;
        }

        public LoggingModule(string appName)
        {
            AppName = appName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            //builder.Register(Component.For<ILog>().LifeStyle.PerWebRequest.UsingFactoryMethod(() => LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)));

            //ILog _log = LogManager.GetLogger(typeof(Program));
            //ILog log = LogManager.GetLogger(typeof(MethodBase));

            WebServicesLogger.Init(""); //TODO: inject the connection string;
            ILog log = LogManager.GetLogger(typeof(MethodBase));

            builder.RegisterInstance(log).As<ILog>();

            //builder.RegisterType<LogManager>()
            //       .As<ILog>()
            //       .InstancePerRequest();

        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            // Handle constructor parameters
            registration.Preparing += OnComponentPreparing;

            // Handle properties
            registration.Activated += (sender, e) => InjectLoggerProperties(e.Instance);
        }

        private static void OnComponentPreparing(object sender, PreparingEventArgs eventArgs)
        {
            eventArgs.Parameters = eventArgs.Parameters.Union(
                                                              new[]
                                                              {
                                                                  new ResolvedParameter(
                                                                                        (p, i) => p.ParameterType == typeof(ILog),
                                                                                        (p, i) => LogManager.GetLogger(p.Member.DeclaringType)
                                                                                       ),
                                                              });
        }

        private static void InjectLoggerProperties(object instance)
        {
            var instanceType = instance.GetType();

            // Get all the injectable properties to set.
            // If you wanted to ensure the properties were only UNSET properties,
            // here's where you'd do it.
            var properties = instanceType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                         .Where(p => p.PropertyType == typeof(ILog) && p.CanWrite && p.GetIndexParameters().Length == 0)
                                         .ToList();

            // Set the properties
            properties.ForEach(p => p.SetValue(instance, LogManager.GetLogger(instanceType), null));
        }

        #endregion
    }
}