using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class EpEmploymentContract
    {
        public int              Id           { get; set; }
        public int EmploymentInformationId { get; set; }
        public int?             JobTypeId    { get; set; }
        public String           JobTypeName  { get; set; }
        public string           JobBeginDate { get; set; }
        public string           JobEndDate   { get; set; }
        public string           JobPosition  { get; set; }
        public string           CompanyName  { get; set; }
        public LocationContract Location     { get; set; }

        // Wage/hours
        public decimal? WageHour   { get; set; }
        public bool     IsSelected { get; set; }
    }
}
