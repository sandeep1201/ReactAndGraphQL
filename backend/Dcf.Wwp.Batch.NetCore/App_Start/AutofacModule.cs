using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Dcf.Wwp.Batch.Interfaces;
using Dcf.Wwp.Batch.Models;

namespace Dcf.Wwp.Batch
{
    public class AutofacModule : Autofac.Module
    {
        #region Properties

        private readonly string _configFileName;

        public string LogLevel      { get; set; }
        public string LogDir        { get; set; }
        public string OutputDir     { get; set; }
        public string Server        { get; set; }
        public string Database      { get; set; }
        public string Uid           { get; set; }
        public string Pwd           { get; set; }
        public string WWP_APP_ENV   { get; set; }
        public string WWP_DB_SERVER { get; set; }
        public string WWP_DB_NAME   { get; set; }
        public string WWP_DB_USER   { get; set; }
        public string WWP_DB_PASS   { get; set; }

        public string Connection { get; set; }

        public bool UseSprocs { get; set; }
        public int  MyInt     { get; set; }

        #endregion

        #region Methods

        public AutofacModule()
        {
        }

        public AutofacModule(string configFileName)
        {
            _configFileName = configFileName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // builder.RegisterType<Test>()
            //        .As<ITest>()
            //        .InstancePerLifetimeScope();

            // builder.RegisterType<DbConfig>()
            //        .As<IDbConfig>()
            //        .InstancePerDependency();

            //builder.RegisterType<JWWWP00>()
            //       .As<IJWWWP00>()
            //       .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   //.Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces()
                   .InstancePerDependency();

            //builder.RegisterType<ExportResultsAsCsv>()
            //       .As<IExportable>()
            //       .InstancePerDependency()
            //       .WithParameters(
            //                       new List<NamedParameter>
            //                       {
            //                           new NamedParameter("logDir", LogDir),
            //                           new NamedParameter("outDir", OutputDir)
            //                       }
            //                      );

            builder.RegisterType<ExportResultsAsCsv>()
                   .As<IExportable>()
                   .WithParameter("logDir", LogDir)
                   .WithParameter("outDir", OutputDir)
                   .InstancePerDependency();

            //TODO: multi-format exports
            //builder.Register<ExportResultsAsCsv>(
            //                                     (c, p) =>
            //                                     {
            //                                         var logDir = p.Named<string>("logDir");

            //                                         if (accountId.StartsWith("9"))
            //                                         {
            //                                             return new GoldCard(accountId);
            //                                         }
            //                                         else
            //                                         {
            //                                             return new StandardCard(accountId);
            //                                         }
            //                                     }).As<IExportable>();

            // done...
            var z = 0;
        }

        #endregion
    }
}
