using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class FamilyMember
    {
        #region Properties

        public int?      FamilyBarriersSectionId { get; set; }
        public int?      RelationshipId          { get; set; }
        public string    FirstName               { get; set; }
        public string    LastName                { get; set; }
        public string    Details                 { get; set; }
        public int?      DeleteReasonId          { get; set; }
        public string    ModifiedBy              { get; set; }
        public DateTime? ModifiedDate            { get; set; }

        #endregion

        #region Navigation Properties

        public virtual FamilyBarriersSection FamilyBarriersSection { get; set; }
        public virtual Relationship          Relationship          { get; set; }
        public virtual DeleteReason          DeleteReason          { get; set; }

        #endregion
    }
}
