using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IFrequency : ICommonModelFinal, ICloneable
    {
        int                                           Id                                 { get; set; }
        string                                        Code                               { get; set; }
        string                                        Name                               { get; set; }
        int                                           SortOrder                          { get; set; }
        string                                        ShortName                          { get; set; }
        ICollection<IActivityScheduleFrequencyBridge> MRActivityScheduleFrequencyBridges { get; set; }
        ICollection<IActivityScheduleFrequencyBridge> WKActivityScheduleFrequencyBridges { get; set; }
    }
}
