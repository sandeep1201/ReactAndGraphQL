using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IExamSubjectType : ICommonModel, ICloneable
    {
        int?                     SortOrder   { get; set; }
        string                   Name        { get; set; }
        int?                     ExamTypeId  { get; set; }
        IExamType                ExamType    { get; set; }
        ICollection<IExamResult> ExamResults { get; set; }
    }
}
