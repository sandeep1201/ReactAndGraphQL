using System;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class POPClaimEmploymentContract
    {
        public int       Id                      {  get; set; }
        public int       POPClaimId              { get;  set; }
        public int       EmploymentInformationId { get;  set; }
        public bool      IsPrimary               { get;  set; }
        public decimal   HoursWorked             { get;  set; }
        public decimal   Earnings                { get;  set; }
        public bool      IsSelected              { get;  set; }
        public int?      JobTypeId               { get;  set; }
        public String    JobTypeName             { get;  set; }
        public string    JobBeginDate            { get;  set; }
        public string    JobEndDate              { get;  set; }
        public string    JobPosition             { get;  set; }
        public string    CompanyName             { get;  set; }
        public string    ModifiedBy              { get;  set; }
        public DateTime? ModifiedDate            { get;  set; }
        public int?      DeletedReasonId         { get;  set; }
        public decimal?  StartingWage            {  get; set; }
        public string    StartingWageUnit        { get;  set; }
    }
}
