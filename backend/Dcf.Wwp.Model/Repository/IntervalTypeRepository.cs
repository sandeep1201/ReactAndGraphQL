using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository:IIntervalTypeRepository
    {
        public IEnumerable<IIntervalType> IntervalTypes()
        {
            var q = from x in _db.IntervalTypes orderby x.SortOrder select x;
            return q;
        }
    }
}
