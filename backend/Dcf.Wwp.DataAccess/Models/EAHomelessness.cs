using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EAHomelessness : BaseEntity
    {
        #region Properties

        public int       RequestId                    { get; set; }
        public bool?     InLackOfPlace                { get; set; }
        public DateTime? DateOfStart                  { get; set; }
        public bool?     PlanOnPermanentPlace         { get; set; }
        public bool?     InShelterForDomesticAbuse    { get; set; }
        public bool?     IsYourBuildingDecidedUnSafe  { get; set; }
        public DateTime? DateBuildingWasDecidedUnSafe { get; set; }
        public bool?     IsInspectionReportAvailable  { get; set; }
        public bool      IsDeleted                    { get; set; }
        public string    ModifiedBy                   { get; set; }
        public DateTime  ModifiedDate                 { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EARequest EaRequest { get; set; }

        #endregion
    }
}
