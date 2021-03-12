using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IActivityScheduleFrequencyBridge : ICommonModel, ICloneable
    {
        int               Id                 { get; set; }
        int?              ActivityScheduleId { get; set; }
        int?              WKFrequencyId      { get; set; }
        int?              MRFrequencyId      { get; set; }
        bool              IsDeleted          { get; set; }
        IActivitySchedule ActivitySchedule   { get; set; }
        IFrequency        MRFrequency        { get; set; }
        IFrequency        WKFrequency        { get; set; }
    }
}
