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
   public partial class Repository:IWageHourRepository
    {
        public IWageHour WageHourById(int? id)
        {
            return (from x in _db.WageHours where x.Id == id where x.IsDeleted == false select x).SingleOrDefault();
        }

        public IWageHour NewWageHour(string user)
        {
            IWageHour wh = new WageHour();
            wh.ModifiedDate = DateTime.Now;
            wh.ModifiedBy = user;
            wh.IsDeleted = false;
            _db.WageHours.Add((WageHour)wh);
            return wh;
        }
    }
}
