using System;
using System.Data.SqlClient;
using Dcf.Wwp.Batch.Interfaces;

namespace Dcf.Wwp.Batch.Infrastructure
{
    public class DbConfig : IDbConfig
    {
        #region Properties

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

        public DbConfig ()
        {
            _server  = Environment.GetEnvironmentVariable("WWP_DB_SERVER");
            _catalog = Environment.GetEnvironmentVariable("WWP_DB_NAME");
            _uid     = Environment.GetEnvironmentVariable("WWP_DB_USER");
            _pwd     = Environment.GetEnvironmentVariable("WWP_DB_PASS");

            Console.WriteLine($"_server = {_server}");
            Console.WriteLine("Ready");
            Console.ReadKey();

            var csb = new SqlConnectionStringBuilder
                      {
                          DataSource               = _server,
                          InitialCatalog           = _catalog,
                          UserID                   = _uid,
                          Password                 = _pwd,
                          MultipleActiveResultSets = true
                      };

            ConnectionString = csb.ConnectionString;
        }

        #endregion
    }
}
