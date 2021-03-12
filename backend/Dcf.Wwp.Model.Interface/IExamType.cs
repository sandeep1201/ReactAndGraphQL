using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IExamType : ICommonModel
    {
        Int32? SortOrder { get; set; }
        String Name { get; set; }
        ICollection<IEducationExam> EducationExams { get; set; }
        ICollection<IExamSubjectMaxScoreType> ExamSubjectMaxScoreTypes { get; set; }
    }
}
