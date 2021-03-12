using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IRepository
    {
        public IEnumerable<IElevatedAccessReason> ElevatedAccessReasons()
        {
            return from x in _db.ElevatedAccessReasons orderby x.SortOrder select x;
        }
    }
}
