using System;
using System.Data.SqlClient;
using Dcf.Wwp.BritsBatch.Interfaces;
using log4net;

namespace Dcf.Wwp.BritsBatch.Infrastructure
{
    public class DbConfig : IDbConfig
    {
        #region Properties

        private readonly ILog _log;

        private string _server          { get; set; }
        private string _catalog         { get; set; }
        private string _uid             { get; set; }
        private string _pwd             { get; set; }
        public  string ConnectionString { get; set; }

        public string Catalog
        {
            get => _catalog;
            set => _catalog = value;
        }

        #endregion

        #region Methods

        public DbConfig (ILog log)
        {
            _log = log;

            _server  = Environment.GetEnvironmentVariable("WWP_DB_SERVER");
            _catalog = Environment.GetEnvironmentVariable("WWP_DB_NAME");
            _uid     = Environment.GetEnvironmentVariable("WWP_DB_USER");
            _pwd     = Environment.GetEnvironmentVariable("WWP_DB_PASS");

            try
            {
                var csb = new SqlConnectionStringBuilder
                          {
                              DataSource               = _server,
                              InitialCatalog           = _catalog,
                              UserID                   = _uid,
                              Password                 = _pwd,
                              MultipleActiveResultSets = true
                          };

                ConnectionString = csb.ConnectionString;

                _log.Info(ToString());
            }
            catch (ArgumentException)
            {
                _log.Debug(ToString());
                _log.Fatal("One of the server system environment variables is not set. Please check the 'WWP_DB_*' env vars.");
                throw;
            }
        }

        public sealed override string ToString() => ($"DataSource: WWP_DB_SERVER:{_server} / WWP_DB_NAME:{ _catalog} / WWP_DB_USER:{_uid}");

        #endregion
    }
}
