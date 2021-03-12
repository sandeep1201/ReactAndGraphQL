using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class FamilyBarriersDetail
    {
        #region Properties

        public string    Details      { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        #endregion
    }
}
