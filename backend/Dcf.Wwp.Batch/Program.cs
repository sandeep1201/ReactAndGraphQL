using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Autofac;
using Autofac.Configuration;
using Fclp;
using log4net;
using log4net.Appender;
using Dcf.Wwp.Batch.Infrastructure;
using Dcf.Wwp.Batch.Interfaces;

namespace Dcf.Wwp.Batch
{
    public class Program
    {
        #region Properties

        private static readonly ILog _log = LogManager.GetLogger("Dcf.Wwp.Batch");

        public static string JobName { get; private set; }
        public static string JobNumber   { get; private set; }

        #endregion

        #region Methods

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit; // to make sure log buffers are flushed and written to disk at exit.

            var exitCode       = 0;
            var appName        = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName);
            var appPath        = AppDomain.CurrentDomain.BaseDirectory;
            var configFileName = Path.Combine(appPath, $"{appName}.json");

            var sw = new Stopwatch();
            sw.Start();

            if (!File.Exists(configFileName))
            {
                exitCode = 105;
                Console.WriteLine($"FATAL: {appName} - Config file {appName}.json is missing - ABEND - exitCode: {exitCode}");
                Environment.Exit(105);
            }

            var batchOptions = ParseCommandLine(args);

            if (batchOptions == null || string.IsNullOrEmpty(batchOptions.JobName))
            {
                exitCode = 110;
                Console.WriteLine($"FATAL: {appName} - parms null or jobName not specified (-j <jobName>) - ABEND - exitCode: {exitCode}");
                Environment.Exit(110);
            }

            JobName = batchOptions.JobName;
            JobNumber   = batchOptions.JobNumber;

            var cb = new ConfigurationBuilder()
                     .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                     .AddEnvironmentVariables("WWP_")
                     .AddJsonFile($"{appName}.json")
                     .Build();

            var batchModule = new ConfigurationModule(cb);

            var autofac = new ContainerBuilder();
            autofac.RegisterInstance(batchOptions).As<IProgramOptions>().AsSelf();
            autofac.RegisterModule(batchModule);

            try
            {
                using (var container = autofac.Build())
                {
                    using (var scope = container.BeginLifetimeScope("WWPJob"))
                    {
                        _log.Info($"Options: {batchOptions}");

                        var job     = scope.ResolveNamed<IBatchJob>(batchOptions.JobName);
                        var file    = scope.ResolveNamed<IExportable>(batchOptions.JobName);

                        _log.Info($"Job Desc: {batchOptions.Desc ?? job.Desc}");

                        var results = job.Run();
                        file.Export(results);
                    }
                }
            }
            catch (SqlException ex)
            {
                exitCode = 130;
                _log.Error(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                _log.Fatal($"ABEND - SqlException - Exit code: {exitCode}");
            }
            catch (Exception ex) // likely a DependencyResolutionException
            {
                exitCode = 120;
                _log.Error(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                _log.Fatal($"ABEND - Exception - Exit code: {exitCode}");
            }

            sw.Stop();

            _log.Info($"Completed in {sw.Elapsed:hh\\:mm\\:ss\\.fff} or {sw.ElapsedMilliseconds} millisecs total");
            _log.Info("Done.");
            _log.Info("END LOG ".PadRight(50, '*'));

            //Credits();

            Environment.Exit(exitCode);
        }

        private static IProgramOptions ParseCommandLine(string[] args)
        {
            _log.Debug("Parsing command line options");

            var parms = new ProgramOptions();

            var parser = new FluentCommandLineParser { IsCaseSensitive = false };

            parser.Setup<string>('j', "jobName")
                  .WithDescription("The name of the Control-M batch job to run e.g. '-j JWWWP01' (required, no default)")
                  .Callback(p => parms.JobName = p.ToUpper())
                  .Required();

            parser.Setup<string>('f', "format")
                  .WithDescription("The format of the output file '-f csv / xlsx / xml' (defaults to *.csv)")
                  .Callback(p => parms.ExportFormat = p.ToLower())
                  .SetDefault("csv");

            parser.Setup<bool>('s', "simulate")
                  .WithDescription("Runs in simulation (read-only) mode (defaults to 'false')")
                  .Callback(p => parms.IsSimulation = p)
                  .SetDefault(false);

            parser.Setup<string>('p', "ProgramCode")
                  .WithDescription("Program Code to set either 'cf' or 'ww'")
                  .Callback(p => parms.ProgramCode = p.ToLower());

            parser.Setup<string>('c', "Sproc")
                  .WithDescription("Set the Stored Procedure name")
                  .Callback(p => parms.Sproc = p);

            parser.Setup<string>('d', "Desc")
                  .WithDescription("Description of the job")
                  .Callback(p => parms.Desc = p);

            parser.Setup<string>('n', "JobNumber")
                  .WithDescription("Number of the Job")
                  .Callback(p => parms.JobNumber = p);

            parser.Setup<bool>('r', "IsRQJ")
                  .WithDescription("Is the job a RQJ")
                  .Callback(p => parms.IsRQJ = p);

            parser.SetupHelp("?", "help")
                  .Callback(ShowHelp);

            var result = parser.Parse(args);

            _log.Info($"Options: {parms}");

            if (!result.HasErrors && !result.HelpCalled)
            {
                return (parms);
            }

            return (null);
        }

        private static void ShowHelp(string helpText)
        {
            var header = "\r\n\tCommand line arguments:\r\n\t--------------------------------------------";
            Console.WriteLine(header);
            Console.WriteLine(helpText);
            Console.WriteLine("\t(c) 2019 - DCF - State of WI");
            Environment.Exit(101);
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
