namespace Dcf.Wwp.DataAccess.Models
{
    public class USP_PrintEP_Info_Result
    {
        #region Properties

        public string   Placement        { get; set; }
        public decimal? CaseNumber       { get; set; }
        public string   AddressType      { get; set; }
        public string   PartAddressLine1 { get; set; }
        public string   PartAddressLine2 { get; set; }
        public string   PartCity         { get; set; }
        public string   PartState        { get; set; }
        public string   PartZipCode      { get; set; }
        public string   PartPhoneNumber  { get; set; }
        public string   HomeLanguage     { get; set; }
        public string   WkrFirstName     { get; set; }
        public string   WkrMiddleInitial { get; set; }
        public string   WkrLastName      { get; set; }
        public string   WkrPhoneNumber   { get; set; }
        public string   WkrEmail         { get; set; }
        public string   AgencyName       { get; set; }
        public string   OrgAddressLine1  { get; set; }
        public string   OrgCity          { get; set; }
        public string   OrgState         { get; set; }
        public string   OrgZipCode       { get; set; }

        #endregion
    }
}
