using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Model.Repository
{
   public partial class Repository
    {
       public IBarrierAccommodation NewBarrierAccommodation(IBarrierDetail parentObject)
       {
           IBarrierAccommodation ba = new BarrierAccommodation();
            ba.BarrierDetail = parentObject;
            _db.BarrierAccommodations.Add((BarrierAccommodation)ba);

            return ba;
        }
    }
}
