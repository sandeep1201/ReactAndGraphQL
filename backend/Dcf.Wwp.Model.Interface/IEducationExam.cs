using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IEducationExam : ICommonModel, ICloneable
    {
        Int32?                   ParticipantId { get; set; }
        Int32?                   ExamTypeId    { get; set; }
        DateTime?                DateTaken     { get; set; }
        String                   Details       { get; set; }
        Boolean                  IsDeleted     { get; set; }
        IExamType                ExamType      { get; set; }
        IParticipant             Participant   { get; set; }
        ICollection<IExamResult> ExamResults   { get; set; }
    }
}
