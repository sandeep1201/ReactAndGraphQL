using System;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class ParticipantStatusContract
    {
        public int       Id                  { get; set; }
        public int?      ParticipantId       { get; set; }
        public decimal?  Pin                 { get; set; }
        public int?      StatusId            { get; set; }
        public string    StatusName          { get; set; }
        public string    StatusCode          { get; set; }
        public string    Details             { get; set; }
        public DateTime? BeginDate           { get; set; }
        public DateTime? EndDate             { get; set; }
        public bool?     IsCurrent           { get; set; }
        public int?      EnrolledProgramId   { get; set; }
        public string    EnrolledProgramName { get; set; }
        public string    EnrolledProgramCode { get; set; }
    }
}
