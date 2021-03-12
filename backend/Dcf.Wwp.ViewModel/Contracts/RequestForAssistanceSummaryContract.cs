using System;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class RequestForAssistanceSummaryContract
    {
        public int       Id                          { get; set; }
        public int       ProgramId                   { get; set; }
        public string    ProgramCode                 { get; set; }
        public string    ProgramName                 { get; set; }
        public decimal?  RfaNumber                   { get; set; }
        public int       StatusId                    { get; set; }
        public string    StatusName                  { get; set; }
        public DateTime? StatusDate                  { get; set; }
        public string    AgencyName                  { get; set; }
        public string    CountyOfResidenceName       { get; set; }
        public string    WorkProgramOfficeCountyName { get; set; }
        public DateTime? ApplicationDate             { get; set; }
        public DateTime? ApplicationDueDate          { get; set; }
        public DateTime? CourtOrderEffectiveDate     { get; set; }
        public DateTime? EnrolledDate                { get; set; }
        public DateTime? DisenrolledDate             { get; set; }
        public int?      ContractorId                { get; set; }
        public string    ContractorName              { get; set; }
        public string    CountyName                  { get; set; }
        public short?    CountyNumber                { get; set; }
        public string    ContractorCode              { get; set; }
        public int?      WorkProgramOfficeNumber     { get; set; }
        public bool      IsVoluntary                 { get; set; }
        public decimal?  KIDSPin                     { get; set; }
        public string    ReferralSource              { get; set; }
    }
}
