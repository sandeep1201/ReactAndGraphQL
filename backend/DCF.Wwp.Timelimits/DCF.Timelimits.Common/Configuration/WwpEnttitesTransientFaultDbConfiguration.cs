using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Runtime.Remoting.Messaging;
using DCF.Common;
using DCF.Common.Configuration;

namespace Dcf.Wwp.Data.Sql
{
    public class WwpEnttitesTransientFaultDbConfiguration : DbConfiguration
    {
        public WwpEnttitesTransientFaultDbConfiguration(IDatabaseConfiguration config)
        {
            this.SetExecutionStrategy("System.Data.SqlClient", () => WwpEnttitesTransientFaultDbConfiguration.SuspendExecutionStrategy ? (IDbExecutionStrategy)new DefaultExecutionStrategy() : (IDbExecutionStrategy)new DcfDbExecutionStrategy(5, TimeSpan.FromSeconds(config.Timeout / 5)));
        }


        public static bool SuspendExecutionStrategy
        {
            get
            {
                return (bool?)CallContext.LogicalGetData("SuspendExecutionStrategy") ?? false;
            }
            set
            {
                CallContext.LogicalSetData("SuspendExecutionStrategy", value);
            }
        }
    }
}