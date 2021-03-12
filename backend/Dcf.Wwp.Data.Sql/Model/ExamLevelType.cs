using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ExamLevelType : BaseEntity
    {
        #region Properties

        public string    Name         { get; set; }
        public string    Code         { get; set; }
        public string    ExamType     { get; set; }
        public int?      SortOrder    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        #endregion
    }
}
