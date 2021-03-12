using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ExamSubjectTypeBridge : BaseEntity
    {
        #region Properties

        public int?      ExamSubjectTypeId { get; set; }
        public int?      ExamTypeId        { get; set; }
        public bool      IsDeleted         { get; set; }
        public string    ModifiedBy        { get; set; }
        public DateTime? ModifiedDate      { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ExamSubjectType ExamSubjectType { get; set; }
        public virtual ExamType        ExamType        { get; set; }

        #endregion
    }
}
