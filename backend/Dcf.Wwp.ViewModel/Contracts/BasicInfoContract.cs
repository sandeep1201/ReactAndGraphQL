using System;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class BasicInfoContract
    {
        public string    FirstName         { get; set; }
        public string    MiddleInitialName { get; set; }
        public string    LastName          { get; set; }
        public string    SuffixName        { get; set; }
        public DateTime? DateOfBirth       { get; set; }
        public int?      Age               { get; set; }
        public decimal?  CaseNumber        { get; set; }
        public decimal?  PinNumber         { get; set; }
        public string    RefugeeCode       { get; set; }
        public DateTime? RefugeeEntryDate  { get; set; }
        public string    GenderIndicator   { get; set; }
        public string    RaceCode          { get; set; }
        public bool?     IsHispanic        { get; set; }
        public string    CountryOfOrigin   { get; set; }
        public string    MFWorkerId        { get; set; }
    }
}
