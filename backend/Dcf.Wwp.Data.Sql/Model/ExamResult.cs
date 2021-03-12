using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ExamResult
    {
        #region Properties

        public int?      EducationExamId       { get; set; }
        public int       ExamSubjectTypeId     { get; set; }
        public DateTime? DatePassed            { get; set; }
        public int?      Score                 { get; set; }
        public int?      MaxScoreRange         { get; set; }
        public int?      SPLTypeId             { get; set; }
        public int?      NRSTypeId             { get; set; }
        public string    Version               { get; set; }
        public int?      ExamEquivalencyTypeId { get; set; }
        public decimal?  GradeEquivalency      { get; set; }
        public int?      ExamLevelType         { get; set; }
        public int?      ExamPassTypeId        { get; set; }
        public string    Level                 { get; set; }
        public string    Form                  { get; set; }
        public string    CasasGradeEquivalency { get; set; }
        public string    ModifiedBy            { get; set; }
        public DateTime? ModifiedDate          { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EducationExam       EducationExam       { get; set; }
        public virtual ExamSubjectType     ExamSubjectType     { get; set; }
        public virtual SPLType             SPLType             { get; set; }
        public virtual NRSType             NRSType             { get; set; }
        public virtual ExamEquivalencyType ExamEquivalencyType { get; set; }
        public virtual ExamPassType        ExamPassType        { get; set; }

        #endregion
    }
}
