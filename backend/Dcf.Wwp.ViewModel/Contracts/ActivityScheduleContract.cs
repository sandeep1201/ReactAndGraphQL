using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class ActivityScheduleContract
    {
        public int                                     Id                          { get; set; }
        public string                                  ScheduleStartDate           { get; set; }
        public bool?                                   IsRecurring                 { get; set; }
        public int?                                    FrequencyTypeId             { get; set; }
        public string                                  FrequencyTypeName           { get; set; }
        public string                                  ScheduleEndDate             { get; set; }
        public string                                  ActualEndDate               { get; set; }
        public string                                  HoursPerDay                 { get; set; }
        public int?                                    BeginHour                   { get; set; }
        public int?                                    BeginMinute                 { get; set; }
        public int?                                    BeginAmPm                   { get; set; }
        public int?                                    EndHour                     { get; set; }
        public int?                                    EndMinute                   { get; set; }
        public int?                                    EndAmPm                     { get; set; }
        public TimeSpan?                               BeginTime                   { get; set; }
        public TimeSpan?                               EndTime                     { get; set; }
        public int?                                    EmployabilityPlanId         { get; set; }
        public List<ActivityScheduleFrequencyContract> ActivityScheduleFrequencies { get; set; }
    }
}
