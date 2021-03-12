using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Autofac;
using Autofac.Configuration;
using Fclp;
using Dcf.Wwp.Batch.Infrastructure;
using Dcf.Wwp.Batch.Interfaces;

namespace Dcf.Wwp.Batch
{
    public class Program
    {
        #region Properties

        //TODO: add logging

        #endregion

        #region Methods

        private static void Main(string[] args)
        {
            //TODO: add logging, try/catch/finally

            var parms = ParseCommandLine(args);

            if (parms == null)
            {
                Environment.Exit(-1);
            }

            var retCode = 0;

            var config = new ConfigurationBuilder()
                         .AddEnvironmentVariables("WWP_")
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("Dcf.Wwp.Batch.NetCore.json");

            var autofac = new ContainerBuilder();

            var batchModule = new ConfigurationModule(config.Build());
            autofac.RegisterModule(batchModule);

            using (var container = autofac.Build())
            {
                using (var scope = container.BeginLifetimeScope("DancingBanana"))
                {
                    IBatchJob job = null;

                    switch (parms.JobName)
                    {
                        case "JWWWP00":
                            job = scope.Resolve<IJWWWP00>();
                            break;

                        case "JWWWP01":
                            job = scope.Resolve<IJWWWP01>();
                            break;

                        default:
                            break;
                    }

                    var dataTable = job?.Run();

                    var fileExporter = scope.Resolve<IExportable>(new NamedParameter("jobName", job.Name));
                    fileExporter.WriteOutput(dataTable);

                    var z = 0;
                }
            }

            Environment.Exit(retCode);
        }

        static AppArguments ParseCommandLine(string[] args)
        {
            var parms = new AppArguments();

            var parser = new FluentCommandLineParser { IsCaseSensitive = false };

            parser.Setup<string>('j', "jobName")
                  .WithDescription("The name of the Control-M batch job to run e.g. '-j JWWWP01'")
                  .Callback(p => parms.JobName = p.ToUpper())
                  .Required();

            parser.Setup<string>('f', "format")
                  .WithDescription("The format of the output file '-f csv / xlsx / xml'")
                  .Callback(p => parms.ExportFormat = p.ToLower())
                  .SetDefault("csv");
                  
            parser.SetupHelp("?", "help")
                  .Callback(ShowHelp);

            var result = parser.Parse(args);

            if (!result.HasErrors && !result.HelpCalled)
            {
                return parms;
            }

            Console.WriteLine(result.ErrorText);

            if (!result.HelpCalled)
            {
                parser.HelpOption.ShowHelp(parser.Options);
            }

            return null;
        }

        private static void ShowHelp(string helpText)
        {
            var header = "\tCommand line arguments:\r\n\t--------------------------------------------";
            Console.WriteLine(header);
            Console.WriteLine(helpText);
            Console.WriteLine("\tPress any key to exit...");
            Console.ReadKey();
        }

        #endregion
    }
}
