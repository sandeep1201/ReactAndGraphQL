using System;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class EventsContract
    {
        public int       Id                  { get; set; }
        public string    Title               { get; set; }
        public string    Type                { get; set; }
        public string    Description         { get; set; }
        public DateTime  StartDate           { get; set; }
        public string    Hours               { get; set; }
        public DateTime? EndDate             { get; set; }
        public string    EndTime             { get; set; }
        public string    StartTime           { get; set; }
        public int?      EmployabilityPlanId { get; set; }
    }
}
