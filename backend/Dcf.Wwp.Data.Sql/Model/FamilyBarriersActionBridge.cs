using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class FamilyBarriersActionBridge : BaseEntity
    {
        #region Properties

        public int?      FamilyBarriersAssessmentSectionId { get; set; }
        public int?      ActionNeededId                    { get; set; }
        public bool      IsDeleted                         { get; set; }
        public string    ModifiedBy                        { get; set; }
        public DateTime? ModifiedDate                      { get; set; }

        #endregion

        #region Navigation Properties

        public virtual FamilyBarriersAssessmentSection FamilyBarriersAssessmentSection { get; set; }

        #endregion
    }
}
