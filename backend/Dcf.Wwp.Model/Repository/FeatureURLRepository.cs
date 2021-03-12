using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IFeatureURLRepository
    {
        public IQueryable<string> GetFeatureUrl(string feature)
        {
            var q = _db.FeatureURLs?.Where(x => x.Feature == feature).Select(y => y.URL);
            return q;
        }
    }
}
