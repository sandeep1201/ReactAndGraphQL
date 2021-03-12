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
      public IWageHourHistoryJobActionBridge NewWageHourHistoryJobActionBridge(IWageHourHistory parentObject, string user)
       {
            //IWageHourHistoryJobActionBridge whhab = new WageHourHistoryJobActionBridge();
            //whhab.WageHourHistory = parentObject;
            //whhab.ModifiedBy = user;
            //whhab.IsDeleted = false;
            //_db.WageHourHistoryJobActionBridges.Add((WageHourHistoryJobActionBridge)whhab);
            //return whhab;

           return null;
       }
    }
}
