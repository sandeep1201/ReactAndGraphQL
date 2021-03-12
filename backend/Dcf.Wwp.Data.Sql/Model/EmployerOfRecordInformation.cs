using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EmployerOfRecordInformation
    {
        #region Properties

        public int       EmploymentInformationId { get; set; }
        public string    CompanyName             { get; set; }
        public string    Fein                    { get; set; }
        public string    StreetAddress           { get; set; }
        public string    ZipAddress              { get; set; }
        public int?      CityId                  { get; set; }
        public int?      JobSectorId             { get; set; }
        public int?      ContactId               { get; set; }
        public bool      IsDeleted               { get; set; }
        public string    ModifiedBy              { get; set; }
        public DateTime? ModifiedDate            { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EmploymentInformation EmploymentInformation { get; set; }
        public virtual City                  City                  { get; set; }
        public virtual Contact               Contact               { get; set; }
        public virtual JobSector             JobSector             { get; set; }

        #endregion
    }
}
