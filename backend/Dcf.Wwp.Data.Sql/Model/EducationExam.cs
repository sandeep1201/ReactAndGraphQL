using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EducationExam
    {
        #region Properties

        public int?      ParticipantId { get; set; }
        public int?      ExamTypeId    { get; set; }
        public DateTime? DateTaken     { get; set; }
        public string    Details       { get; set; }
        public string    ModifiedBy    { get; set; }
        public DateTime? ModifiedDate  { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ExamType                ExamType    { get; set; }
        public virtual Participant             Participant { get; set; }
        public virtual ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();

        #endregion
    }
}
