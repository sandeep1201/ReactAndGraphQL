using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Dcf.Wwp.Data.Sql
{
    public class WwpEnttitesDbConfiguration : DbConfiguration
    {
        public WwpEnttitesDbConfiguration()
        {
            //SetExecutionStrategy("System.Data.SqlClient", config.ExecutionStrategyFactory);
            SetManifestTokenResolver(new DcfSqlSvrManifestTokenResolver());
        }
    }

    public class DcfSqlSvrManifestTokenResolver : IManifestTokenResolver
    {
        public string ResolveManifestToken(DbConnection connection)
        {
            return "2014";
        }
    }

}
