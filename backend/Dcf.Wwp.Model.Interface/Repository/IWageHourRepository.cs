using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface.Repository
{
   public interface IWageHourRepository
   {
       IWageHour WageHourById(Int32? id);
       IWageHour NewWageHour(String user);
   }
}
