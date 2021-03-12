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
   public partial class Repository:IWageHourHistoryRepository
    {
        public IWageHourHistory NewWageHourHistory(IWageHour parentAssessment, string user)
        {
            IWageHourHistory whh = new WageHourHistory();
            whh.WageHour = parentAssessment;
            whh.ModifiedBy = user;
            whh.ModifiedDate = DateTime.Now;
            whh.IsDeleted = false;

            _db.WageHourHistories.Add((WageHourHistory)whh);

            return whh;
        }
    }
}
