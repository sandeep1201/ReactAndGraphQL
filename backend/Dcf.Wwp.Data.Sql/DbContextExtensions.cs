using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Core.Dependency;
using Dcf.Wwp.Data.Sql.Model;

namespace System.Data.Entity
{
    public abstract class DbContextFactory<T> : IServiceFactory<T> where T : DbContext
    {
        public abstract T Build();
    }

    public class WWpEntitiesContextFactory : DbContextFactory<WwpEntities>
    {
        public WWpEntitiesContextFactory()
        {
        }

        public override WwpEntities Build()
        {
            throw new NotImplementedException();
        }
    }
}
