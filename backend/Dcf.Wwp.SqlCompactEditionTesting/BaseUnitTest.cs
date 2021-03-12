using System.Data.Common;
using System.IO;
using Dcf.Wwp.Data;
using Dcf.Wwp.Data.Sql.Model;
using System.Data.Entity.Infrastructure;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;

namespace Dcf.Wwp.SqlCompactEditionTesting
{
	public abstract class BaseUnitTest
	{
		public WwpEntities Db { get; private set; }
		public SqlCeConnectionFactory Connection { get; private set; }

		protected BaseUnitTest()
		{
			Intialize();
		}
		public void Intialize()
		{
			//#if TESTUNITSCE
			string currentFile = Path.GetTempFileName();
			string connectionString = $"Data Source={currentFile};Persist Security Info=False";
			Connection = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
			var conn = DbProviderFactories.GetFactory("System.Data.SqlServerCe.4.0").CreateConnection();			
			conn.ConnectionString = connectionString;
			conn.Open();
			Db = new WwpEntities(conn);
			Db.Database.CreateIfNotExists();
		}
	}
}
