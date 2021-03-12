using System.Linq;
using System.Reflection;
using Autofac.Core;
using log4net;

namespace Dcf.Wwp.BritsBatch
{
    public class LoggingModule : Autofac.Module
    {
        #region Properties

        public string LogLevel   { get; set; }
        public string LogPath    { get; set; }
        public string JobLogPath { get; set; }

        #endregion

        #region Methods

        private static void InjectLoggerProperties(object instance)
        {
            var instanceType = instance.GetType();

            // Get all the injectable properties to set. If you wanted to ensure the
            // properties were only UNSET properties, here's where you'd do it.
            var properties = instanceType
                             .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                             .Where(p => p.PropertyType == typeof(ILog) && p.CanWrite && p.GetIndexParameters().Length == 0);

            // Set the properties located.
            foreach (var p in properties)
            {
                p.SetValue(instance, LogManager.GetLogger(instanceType), null);
            }
        }

        private static void OnComponentPreparing(object sender, PreparingEventArgs eventArgs)
        {
            eventArgs.Parameters = eventArgs.Parameters.Union(
                                                              new[]
                                                              {
                                                                  new ResolvedParameter(
                                                                                        (p, i) => p.ParameterType == typeof(ILog),
                                                                                        (p, i) => LogManager.GetLogger("Dcf.Wwp.BritsBatch")
                                                                                       )
                                                              });
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            // Handle constructor parameters.
            registration.Preparing += OnComponentPreparing;

            // Handle properties.
            registration.Activated += (sender, e) => InjectLoggerProperties(e.Instance);
        }

        #endregion
    }
}
