using System;
using Dcf.Wwp.Api.Library.Contracts.ActionNeeded;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class WeeklyHoursWorkedContract
    {
        public WeeklyHoursWorkedContract()
        {
        }

        public int      Id                      { get; set; }
        public int      EmploymentInformationId { get; set; }
        public DateTime StartDate               { get; set; }
        public decimal  Hours                   { get; set; }
        public string   Details                 { get; set; }
        public decimal  TotalSubsidyAmount      { get; set; }
        public decimal  TotalWorkSiteAmount     { get; set; }
        public bool     IsDeleted               { get; set; }
        public string   ModifiedBy              { get; set; }
        public DateTime ModifiedDate            { get; set; }
    }
}
