using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IActivitySchedule : ICommonModelFinal, ICloneable
    {
        int                                           Id                               { get; set; }
        int                                           ActivityId                       { get; set; }
        DateTime?                                     StartDate                        { get; set; }
        bool?                                         IsRecurring                      { get; set; }
        int?                                          FrequencyTypeId                  { get; set; }
        DateTime?                                     PlannedEndDate                   { get; set; }
        DateTime?                                     ActualEndDate                    { get; set; }
        decimal?                                      HoursPerDay                      { get; set; }
        TimeSpan?                                     BeginTime                        { get; set; }
        TimeSpan?                                     EndTime                          { get; set; }
        int?                                          EmployabilityPlanId              { get; set; }
        IActivity                                     Activity                         { get; set; }
        IFrequencyType                                FrequencyType                    { get; set; }
        ICollection<IActivityScheduleFrequencyBridge> ActivityScheduleFrequencyBridges { get; set; }
    }
}
