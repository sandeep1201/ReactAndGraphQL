namespace Dcf.Wwp.Api.Library.Contracts
{
    public class EmployerOfRecordDetailContract
    {
        public LocationContract Location           { get; set; }
        public string           CompanyName        { get; set; }
        public string           Fein               { get; set; }
        public int?             JobSectorId        { get; set; }
        public string           JobSectorName      { get; set; }
        public int?             ContactId          { get; set; }
        public int?             Id                 { get; set; }
    }
}
