using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IExamResult : ICommonModel, ICloneable
    {
        float?           ScorePercentage       { get; set; }
        int?             EducationExamId       { get; set; }
        int              ExamSubjectTypeId     { get; set; }
        int?             NRSTypeId             { get; set; }
        int?             SPLTypeId             { get; set; }
        int?             MaxScoreRange         { get; set; }
        int?             ExamLevelType         { get; set; }
        int?             ExamEquivalencyTypeId { get; set; }
        decimal?         GradeEquivalency      { get; set; }
        string           Version               { get; set; }
        DateTime?        DatePassed            { get; set; }
        bool             IsDeleted             { get; set; }
        int?             Score                 { get; set; }
        int?             ExamPassTypeId        { get; set; }
        string           Level                 { get; set; }
        string           Form                  { get; set; }
        string           CasasGradeEquivalency { get; set; }
        INRSType         NRSType               { get; set; }
        ISPLType         SPLType               { get; set; }
        IExamSubjectType ExamSubjectType       { get; set; }
        IEducationExam   EducationExam         { get; set; }

        IExamPassType ExamPassType { get; set; }

        //IExamVersionType ExamVersionType { get; set; }
        IExamEquivalencyType ExamEquivalencyType { get; set; }
    }
}
