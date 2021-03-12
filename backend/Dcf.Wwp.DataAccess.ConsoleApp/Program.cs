using System;
using System.Linq;
using System.Text;
using Autofac;
using Dcf.Wwp.DataAccess.Contexts;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Dcf.Wwp.DataAccess.ConsoleApp
{
    class Program
    {
        #region Properties

        //private static readonly ILog _log = LogManager.GetLogger("Dcf.Wwp.DataAccess.ConsoleApp");
        private static readonly ILog _log = LogManager.GetLogger(typeof(Program));

        #endregion

        #region Methods

        static void Main(string[] args)
        {
            SetupLog();

            try
            {
                // using (var db = new EPContext(@"Data Source=DBWMAD0D2613, 2025;Database=WWPDEV;Integrated Security=True;MultipleActiveResultSets=True"))
                // {
                var autofac = new ContainerBuilder();
                autofac.RegisterModule(new AutofacModule());

                using (var container = autofac.Build())
                {
                    using (var scope = container.BeginLifetimeScope("DancingBanana"))
                    {
                        // 42	171	1	dsadsad	3	NULL	False	alallsxgrl	2019-01-04 14:29:31.867	<Binary data>
                        // 171	11314	1	False	2019-01-04 00:00:00.000	2019-05-10 00:00:00.000	NULL	False	Sandeep Reddy Alalla	2019-01-04 14:28:17.890	<Binary data>	NULL	NULL

                        var r0 = scope.Resolve<IOrganizationRepository>();
                        var r1 = scope.Resolve<IWorkerRepository>();
                        var r2 = scope.Resolve<IEnrolledProgramRepository>();

                        var v1 = "ROSS IES W-2 PR ";
                        var v2 = "ROSS IES W-2 PR \r\n";

                        var c = (EPContext)scope.Resolve<IDbContext>();

                        var d = c.Set<Organization>();
                        var b = d.FirstOrDefault(i => i.DB2AgencyName.Equals(v2));
                        var a = r0.Get(i => i.DB2AgencyName == v2);

                        Console.WriteLine(a);
                        //var x = uow.Commit();
                        var z = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                _log.Debug(ex);
            }

            Environment.Exit(0);
        }

        private static void SetupLog()
        {
            BasicConfigurator.Configure();

            var hierarchy = (Hierarchy) LogManager.GetRepository();
            var log       = LogManager.GetLogger("Dcf.Wwp.DataAccess");
            var logger    = (Logger) log.Logger;

            var ra = (ConsoleAppender) hierarchy.Root.Appenders[0];

            var layout = new PatternLayout("%date{MM-dd-yyyy HH:mm:ss tt} - %logger -  %-5level - %message %newline");
            layout.ActivateOptions();

            ra.Layout = layout;

            logger.Parent     = hierarchy.Root;
            logger.Level      = Level.Debug;
            logger.Additivity = true;

            var fileAppender = new FileAppender
                               {
                                   File           = "logs\\Dcf.Wwp.DataAccess.ConsoleApp.log",
                                   AppendToFile   = false,
                                   Layout         = layout,
                                   ImmediateFlush = true,
                                   Threshold      = Level.Debug
                               };

            fileAppender.ActivateOptions();

            logger.AddAppender(fileAppender);

            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            _log.Debug("shutting down");
            _log.Debug("flushing log buffers");

            (LogManager.GetRepository()
                       .GetAppenders()
                       .ToList())
                .ForEach(a =>
                         {
                             if (a is BufferingAppenderSkeleton buffered)
                             {
                                 buffered.Flush();
                             }
                         });
        }

        #endregion
    }
}
