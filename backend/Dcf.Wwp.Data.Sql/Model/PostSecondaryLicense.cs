using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class PostSecondaryLicense
    {
        #region Properties

        public string    Name                            { get; set; }
        public string    Issuer                          { get; set; }
        public DateTime? AttainedDate                    { get; set; }
        public DateTime? ExpiredDate                     { get; set; }
        public bool?     IsInProgress                    { get; set; }
        public bool?     DoesNotExpire                   { get; set; }
        public int?      ValidInWIPolarLookupId          { get; set; }
        public int?      LicenseTypeId                   { get; set; }
        public int       PostSecondaryEducationSectionId { get; set; }
        public int?      OriginId                        { get; set; }
        public bool      IsDeleted                       { get; set; }
        public string    ModifiedBy                      { get; set; }
        public DateTime? ModifiedDate                    { get; set; }

        #endregion

        #region Navigation Properties

        public virtual PolarLookup                   PolarLookup                   { get; set; }
        public virtual LicenseType                   LicenseType                   { get; set; }
        public virtual PostSecondaryEducationSection PostSecondaryEducationSection { get; set; }

        #endregion
    }
}
