using System;
using Dcf.Wwp.DataAccess.Base;
using Newtonsoft.Json;

namespace Dcf.Wwp.DataAccess.Models
{
    public class OrganizationLocation : BaseEntity
    {
        #region Properties

        public int       OrganizationInformationId       { get; set; }
        public string    AddressLine1                    { get; set; }
        public int       CityId                          { get; set; }
        public string    ZipCode                         { get; set; }
        public int       AddressVerificationTypeLookupId { get; set; }
        public DateTime  EffectiveDate                   { get; set; }
        public DateTime? EndDate                         { get; set; }
        public bool      IsDeleted                       { get; set; }
        public string    ModifiedBy                      { get; set; }
        public DateTime  ModifiedDate                    { get; set; }

        #endregion

        #region Navigation Properties

        public virtual OrganizationInformation       OrganizationInformation       { get; set; }
        public virtual City                          City                          { get; set; }
        public virtual AddressVerificationTypeLookup AddressVerificationTypeLookup { get; set; }

        #endregion
    }
}
