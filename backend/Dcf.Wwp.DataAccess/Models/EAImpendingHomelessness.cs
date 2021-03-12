using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EAImpendingHomelessness : BaseEntity
    {
        #region Properties

        public int       RequestId                                { get; set; }
        public bool?     HaveEvictionNotice                       { get; set; }
        public DateTime? DateOfEvictionNotice                     { get; set; }
        public string    DifficultToPayDetails                    { get; set; }
        public bool?     IsCurrentLandLordUnknown                 { get; set; }
        public string    LandLordName                             { get; set; }
        public string    ContactPerson                            { get; set; }
        public string    LandLordPhone                            { get; set; }
        public string    LandLordAddress                          { get; set; }
        public int?      LandLordCityId                           { get; set; }
        public string    LandLordZip                              { get; set; }
        public int?      AddressVerificationTypeLookupId          { get; set; }
        public bool?     NeedingDifferentHomeForAbuse             { get; set; }
        public bool?     NeedingDifferentHomeForRentalForeclosure { get; set; }
        public DateTime? DateOfFamilyDeparture                    { get; set; }
        public bool?     IsYourBuildingDecidedUnSafe              { get; set; }
        public DateTime? DateBuildingWasDecidedUnSafe             { get; set; }
        public bool?     IsInspectionReportAvailable              { get; set; }
        public bool      IsDeleted                                { get; set; }
        public string    ModifiedBy                               { get; set; }
        public DateTime  ModifiedDate                             { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EARequest                     EaRequest                     { get; set; }
        public virtual City                          City                          { get; set; }
        public virtual AddressVerificationTypeLookup AddressVerificationTypeLookup { get; set; }

        #endregion
    }
}
