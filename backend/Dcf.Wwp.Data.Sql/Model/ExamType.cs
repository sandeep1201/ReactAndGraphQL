using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ExamType
    {
        #region Properties

        public string    Name         { get; set; }
        public string    FullName     { get; set; }
        public int?      SortOrder    { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<ExamSubjectTypeBridge>   ExamSubjectTypeBridges   { get; set; }
        public virtual ICollection<ExamSubjectMaxScoreType> ExamSubjectMaxScoreTypes { get; set; }
        public virtual ICollection<EducationExam>           EducationExams           { get; set; }
        public virtual ICollection<ExamSubjectType>         ExamSubjectTypes         { get; set; }

        #endregion
    }
}
