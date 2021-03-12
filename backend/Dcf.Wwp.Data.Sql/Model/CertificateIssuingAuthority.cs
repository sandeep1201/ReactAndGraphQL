using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class CertificateIssuingAuthority
    {
        #region Properties

        public string    Code         { get; set; }
        public string    Name         { get; set; }
        public int       SortOrder    { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        #endregion
    }
}
