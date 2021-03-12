using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
   public partial class Repository : IBarrierTypeRepository
    {
       public IEnumerable<IBarrierType> BarrierTypes()
       {
           return (from x in _db.BarrierTypes orderby x.SortOrder  select x);
       }

        public IBarrierType BarrierTypeById(int? id)
        {
            var barrierType = (from bt in _db.BarrierTypes where bt.Id == id select bt).FirstOrDefault();
            return barrierType;
        }

        public IEnumerable<IBarrierType> GetBarrierTypes()
        {
            return (from x in _db.BarrierTypes orderby x.SortOrder select x);
        }
    }
}
