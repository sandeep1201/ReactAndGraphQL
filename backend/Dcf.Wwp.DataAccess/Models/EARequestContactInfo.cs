using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EARequestContactInfo : BaseEntity
    {
        #region Properties

        public int      RequestId                       { get; set; }
        public int?     CountyOfResidenceId             { get; set; }
        public string   AddressLine1                    { get; set; }
        public string   AddressLine2                    { get; set; }
        public string   ZipCode                         { get; set; }
        public int?     CityAddressId                   { get; set; }
        public bool?    HomelessIndicator               { get; set; }
        public bool?    IsHouseHoldMailingAddressSame   { get; set; }
        public int?     AddressVerificationTypeLookupId { get; set; }
        public int?     AlternateMailingAddressId       { get; set; }
        public string   PhoneNumber                     { get; set; }
        public bool?    CanTextPhone                    { get; set; }
        public string   AlternatePhoneNumber            { get; set; }
        public bool?    CanTextAlternatePhone           { get; set; }
        public string   EmailAddress                    { get; set; }
        public string   BestWayToReach                  { get; set; }
        public bool     IsDeleted                       { get; set; }
        public DateTime CreatedDate                     { get; set; }
        public string   ModifiedBy                      { get; set; }
        public DateTime ModifiedDate                    { get; set; }

        #endregion

        #region Navigation Properties

        public virtual City                          City                          { get; set; }
        public virtual CountyAndTribe                CountyAndTribe                { get; set; }
        public virtual EARequest                     EaRequest                     { get; set; }
        public virtual EAAlternateMailingAddress     EAAlternateMailingAddress     { get; set; }
        public virtual AddressVerificationTypeLookup AddressVerificationTypeLookup { get; set; }

        #endregion
    }
}
