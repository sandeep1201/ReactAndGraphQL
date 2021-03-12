using System;

namespace Dcf.Wwp.DataAccess.Models
{
    public class USP_Create_Events
    {
        #region Properties

        public int       Id                  { get; set; }
        public int?      ParticipantId       { get; set; }
        public int?      ActivityId          { get; set; }
        public int       ScheduleId          { get; set; }
        public DateTime? EPBeginDate         { get; set; }
        public DateTime? EPEndDate           { get; set; }
        public DateTime  StartDate           { get; set; }
        public Boolean?  IsRecurring         { get; set; }
        public string    FrequencyType       { get; set; }
        public string    WKFrequency         { get; set; }
        public string    MRFrequency         { get; set; }
        public DateTime? PlannedEndDate      { get; set; }
        public decimal?  HoursPerDay         { get; set; }
        public int?      EmployabilityPlanId { get; set; }
        public string    Title               { get; set; }
        public string    Type                { get; set; }
        public string    Description         { get; set; }
        public TimeSpan? EndTime             { get; set; }
        public TimeSpan? StartTime           { get; set; }
    }

    #endregion
}
