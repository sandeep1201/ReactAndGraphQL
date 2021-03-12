using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ChildYouthSectionChild
    {
        #region Properties

        public int?      ChildYouthSectionId { get; set; }
        public int?      ChildId             { get; set; }
        public int?      CareArrangementId   { get; set; }
        public int?      AgeCategoryId       { get; set; }
        public bool?     IsSpecialNeeds      { get; set; }
        public string    Details             { get; set; }
        public int?      DeleteReasonId      { get; set; }
        public string    ModifiedBy          { get; set; }
        public DateTime? ModifiedDate        { get; set; }

        #endregion

        #region Navigation Properties

        public virtual AgeCategory          AgeCategory          { get; set; }
        public virtual Child                Child                { get; set; }
        public virtual ChildYouthSection    ChildYouthSection    { get; set; }
        public virtual ChildCareArrangement ChildCareArrangement { get; set; }
        public virtual DeleteReason         DeleteReason         { get; set; }

        #endregion
    }
}
