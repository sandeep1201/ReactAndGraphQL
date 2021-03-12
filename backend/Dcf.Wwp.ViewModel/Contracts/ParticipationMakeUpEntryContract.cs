using System;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class ParticipationMakeUpEntryContract
    {
        public int      Id                   { get; set; }
        public int      ParticipationEntryId { get; set; }
        public DateTime MakeupDate           { get; set; }
        public string   MakeupHours          { get; set; }
        public string   ModifiedBy           { get; set; }
        public DateTime ModifiedDate         { get; set; }
    }
}
