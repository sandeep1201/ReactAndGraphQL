namespace Dcf.Wwp.Api.Library.Contracts
{
    public class WwpOfficeContract : BaseContract
    {
        #region Properties

        public int?   OfficeNumber     { get; set; }
        public string OfficeName       { get; set; }
        public short? CountyNumber     { get; set; }
        public string CountyName       { get; set; }
        public string OrganizationCode { get; set; }
        public string OrganizationName { get; set; }
        public string ProgramShortName { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
