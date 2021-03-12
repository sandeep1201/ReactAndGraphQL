using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IWageAction : ICommonDelModel, ICloneable
    {
        String Name { get; set; }
        String ActionType { get; set; }
        Boolean? IsRequired { get; set; }
        Int32? SortOrder { get; set; }

        // Leaving off the nav properties as they are not needed in the interface
        //ICollection<IWageHourHistoryJobActionBridge> WageHourHistoryJobActionBridges { get; set; }
        //ICollection<IWageHourJobActionBridge> WageHourJobActionBridges { get; set; }
    }
}
