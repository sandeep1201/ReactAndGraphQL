using System;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class EmploymentVerificationContract
    {
        public int                     Id                         { get; set; }
        public int                     ParticipantId              { get; set; }
        public int?                    JobTypeId                  { get; set; }
        public string                  JobTypeName                { get; set; }
        public DateTime?               JobBeginDate               { get; set; }
        public DateTime?               JobEndDate                 { get; set; }
        public string                  JobPosition                { get; set; }
        public decimal?                AvgWeeklyHours             { get; set; }
        public string                  CompanyName                { get; set; }
        public FinalistAddressContract Location                   { get; set; }
        public int?                    EmploymentVerificationId   { get; set; }
        public bool?                   IsVerified                 { get; set; }
        public string                  ModifiedBy                 { get; set; }
        public DateTime?               ModifiedDate               { get; set; }
        public DateTime?               CreatedDate                { get; set; }
        public int?                    NumberOfDaysAtVerification { get; set; }
    }
}
