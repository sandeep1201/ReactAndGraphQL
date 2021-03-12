using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IFrequencyType : ICommonModelFinal, ICloneable
    {
        int                            Id                { get; set; }
        string                         Name              { get; set; }
        int                            SortOrder         { get; set; }
        ICollection<IActivitySchedule> ActivitySchedules { get; set; }
    }
}
