using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCF.Common.Configuration;

namespace Dcf.Wwp.Data.Sql
{
    public class WwpEnttitesDbConfiguration : DbConfiguration
    {
        public WwpEnttitesDbConfiguration(DatabaseConfiguration config)
        {
            //SetExecutionStrategy("System.Data.SqlClient", config.ExecutionStrategyFactory);
        }
    }
}
 