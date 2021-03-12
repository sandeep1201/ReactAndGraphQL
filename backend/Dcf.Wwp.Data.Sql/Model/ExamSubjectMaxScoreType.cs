using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ExamSubjectMaxScoreType
    {
        #region Properties

        public int       Id                { get; set; }
        public int?      ExamTypeId        { get; set; }
        public int?      ExamSubjectTypeId { get; set; }
        public string    MaxScore          { get; set; }
        public DateTime? CreatedDate       { get; set; }
        public bool      IsDeleted         { get; set; }
        public string    ModifiedBy        { get; set; }
        public DateTime? ModifiedDate      { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ExamType ExamType { get; set; }

        #endregion
    }
}
