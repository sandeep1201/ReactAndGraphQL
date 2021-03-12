using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Model.Repository
{
   public partial class Repository
    {
        public IEnumerable<IBarrierSubtype> BarrierSubtypeByBarriertype(int? id)
        {
            return (from x in _db.BarrierSubtypes  where x.BarrierTypeId == id select x);
        }
    }
}
