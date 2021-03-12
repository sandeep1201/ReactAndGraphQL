using System;
using System.Data.Common;
using System.IO;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model;
using System.Data.Entity.Infrastructure;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Reflection;
using Dcf.Wwp.Data.Sql.Model;
using Moq;

namespace Dcf.Wwp.ViewModel.UnitTests
{
	public abstract class BaseUnitTest
	{

		public Mock<T> NewMock<T>() where T : class
		{
			var type                                   = typeof(T);
			var constructors                           = type.GetConstructors();
			if (constructors.Length == 0) constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			if (constructors.Length == 0) return new Mock<T>();

			var constructor = constructors[0];
			var parameters  = constructor.GetParameters();

			var objects = parameters
			              .Select(p => p.ParameterType.IsValueType ? Activator.CreateInstance(p.ParameterType) : null)
			              .ToArray();
			var constructorInfo = typeof(Mock<T>).GetConstructor(new Type[] { typeof(object[]) });
			return (Mock<T>)constructorInfo.Invoke(new object[] { objects });
		}

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
			//conn.Open();
			Db = new WwpEntities(conn);
			Db.Database.CreateIfNotExists();
		}
	}
}
