using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
   public interface IIntervalType:ICommonModel, ICloneable
    {
        String Name { get; set; }
        Int32? SortOrder { get; set; }
        Boolean IsDeleted { get; set; }
        ICollection<IWageHour> BeginRateIntervalTypes { get; set; }
        ICollection<IWageHour> CurrentPayRateIntervalTypes { get; set; }
        ICollection<IWageHour> EndRateIntervalTypes { get; set; }
        ICollection<IWageHourHistory> WageHourHistories { get; set; }

    }
}
