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
       public IBarrierTypeBarrierSubTypeBridge NewBarrierTypeBarrierSubTypeBridge(IBarrierDetail parentObject,
           string user)
       {
            IBarrierTypeBarrierSubTypeBridge btsubt = new BarrierTypeBarrierSubTypeBridge();
            btsubt.BarrierDetail = parentObject;
            btsubt.ModifiedBy = user;
            btsubt.ModifiedDate = DateTime.Now;
            btsubt.IsDeleted = false;
            _db.BarrierTypeBarrierSubTypeBridges.Add((BarrierTypeBarrierSubTypeBridge)btsubt);
            return btsubt;
        }
    }
}
