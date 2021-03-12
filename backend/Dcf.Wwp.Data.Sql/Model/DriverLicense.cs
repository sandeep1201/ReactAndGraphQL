using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class DriverLicense
    {
        #region Properties

        public int?      StateId             { get; set; }
        public int?      DriverLicenseTypeId { get; set; }
        public DateTime? ExpiredDate         { get; set; } // just call it 'Expiration Date' already ~ lol
        public string    Details             { get; set; }
        public bool      IsDeleted           { get; set; }
        public string    ModifiedBy          { get; set; }
        public DateTime? ModifiedDate        { get; set; }

        #endregion

        #region Navigation Properties

        public virtual DriverLicenseType DriverLicenseType { get; set; }
        public virtual State             State             { get; set; }

        #endregion
    }
}
